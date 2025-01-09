using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDFFormsApp
{
    public partial class MyForm : Form
    {
        private DateTime? PollTime = DateTime.MinValue;
        private int InsRef = 0;

        private System.Windows.Forms.Timer? PollTimer;
        private OrderBook? WOrderBook;
        public MyForm(int insRef)
        {
            InsRef = insRef;
            InitializeComponent();
        }

        private void MyForm_Load(object sender, EventArgs e)
        {
            labelInsRef.Text = InsRef.ToString();
            this.Text = labelInsRef.Text;
            GetInstrumentName(InsRef);

            PollTime = DateTime.Now;

            WOrderBook = Program.orderBookRepository.GetOrderBook(InsRef);

            if (WOrderBook != null)
            {
                PollTimer = new System.Windows.Forms.Timer();

                PollTimer.Interval = 100;
                PollTimer.Tick += Poll;
                PollTimer.Start();
            }
        }

        private void Poll(object? Sender, EventArgs? e)
        {
            Label[] labelBN = { labelBN1, labelBN2, labelBN3, labelBN4, labelBN5 };
            Label[] labelBQ = { labelBQ1, labelBQ2, labelBQ3, labelBQ4, labelBQ5 };
            Label[] labelBP = { labelBP1, labelBP2, labelBP3, labelBP4, labelBP5 };
            Label[] labelAN = { labelAN1, labelAN2, labelAN3, labelAN4, labelAN5 };
            Label[] labelAQ = { labelAQ1, labelAQ2, labelAQ3, labelAQ4, labelAQ5 };
            Label[] labelAP = { labelAP1, labelAP2, labelAP3, labelAP4, labelAP5 };

            List<OrderBookEntry> bids = WOrderBook.GetBids();
            List<OrderBookEntry> asks = WOrderBook.GetAsks();

            for (int i = 0; i < 5; i++)
            {
                bool noBid = i >= bids.Count;
                bool noAsk = i >= asks.Count;
                labelBN[i].Text = noBid ? "-" : bids[i].NumOrders;
                labelBQ[i].Text = noBid ? "-" : bids[i].Quantity;
                labelBP[i].Text = noBid ? "-" : bids[i].Price;
                labelAN[i].Text = noAsk ? "-" : asks[i].NumOrders;
                labelAQ[i].Text = noAsk ? "-" : asks[i].Quantity;
                labelAP[i].Text = noAsk ? "-" : asks[i].Price;
            }

            List<Trade> newTrades = Program.tradeRepository.GetFromTime(InsRef, PollTime);
            foreach (Trade trade in newTrades)
            {
                string s = trade.Display();
                listTrades.Items.Add(s);

                if (trade.Time > PollTime)
                {
                    PollTime = trade.Time;
                }
            }

            if (checkLockList.Checked)
            {
                listTrades.TopIndex = listTrades.Items.Count - 1;
            }

            labelUpd.Text = DateTime.Now.ToString();
        }

        private async void GetInstrumentName(int insRef)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = string.Format("https://sandbox.millistream.com/mws.fcgi?usr=sandbox&pwd=sandbox&cmd=quote&insref={0}&fields=name,tradecurrency&filetype=json", insRef);

                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    string[] fields = responseBody.Split('\n');
                    string name = GetFieldValue(fields[2]);
                    string tradeCurrency = GetFieldValue(fields[3]);
                    labelInsRef.Text = string.Format("{0} ({1})", name, tradeCurrency);
                    this.Text = name;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("No Instrument found for insref {0}", insRef), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
            }
        }

        private string GetFieldValue(string field)
        {
            return field.Replace(',', ' ').Split(':')[1].Replace('\"', ' ').Trim();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkLockList.Checked && listTrades.Items.Count > 0)
            {
                listTrades.TopIndex = listTrades.Items.Count - 1;
            }
        }
    }
}
