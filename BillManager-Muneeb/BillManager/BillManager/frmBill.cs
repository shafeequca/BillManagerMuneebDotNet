using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BillManager
{
    public partial class frmBill : Form
    {
        public frmBill()
        {
            InitializeComponent();
        }



        private void frmBill_Load(object sender, EventArgs e)
        {
            comboLoad();
            DtPicker.Value = DateTime.Today;
        }
        private void comboLoad()
        {
            try
            {
                
                string query = "select ID,ItemName from Items order By ItemName";
                cboItem.DataSource = Connections.Instance.ShowDataInGridView(query);
                cboItem.DisplayMember = "ItemName";
                cboItem.ValueMember = "ID";
                cboItem.SelectedIndex = -1;
                cboItem.Text = "";

                
                query = "select ID,Party from Party order by Party";
                cboParty.DataSource = Connections.Instance.ShowDataInGridView(query);
                cboParty.DisplayMember = "Party";
                cboParty.ValueMember = "ID";
                cboParty.Text = "";
                cboParty.SelectedIndex = -1;

            }
            catch (Exception ex)
            {

            }
        }
        private void cboParty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboParty.Text != "System.Data.DataRowView" && cboParty.SelectedIndex >= 0)
                {
                    string Query = "select ID,Add1,GSTIN,Phone,State,StateCode from Party where ID=" + cboParty.SelectedValue + "";
                    DataTable Dt = new DataTable();
                    Dt = (DataTable)Connections.Instance.ShowDataInGridView(Query);
                    txtAddress.Text = Dt.Rows[0][1].ToString();
                    txtGSTIN.Text = Dt.Rows[0][2].ToString();
                    txtPhone.Text = Dt.Rows[0][3].ToString();
                    txtState.Text = Dt.Rows[0][4].ToString();
                    txtStateCode.Text = Dt.Rows[0][5].ToString();
                }

            }
            catch (Exception ex)
            {
                txtAddress.Text = "";
                txtGSTIN.Text = "";
                txtPhone.Text = "";
                txtState.Text = "";
                txtStateCode.Text = "";
                cboParty.Text = "";
                cboParty.SelectedIndex = -1;

            }

        }

        private void cboParty_SelectionChangeCommitted(object sender, EventArgs e)
        {

           
 
        }

        private void cmdItemClear_Click(object sender, EventArgs e)
        {
            txtHSN.Text = "";
            cboItem.Text = "";
            cboItem.SelectedIndex = -1;
            txtQty.Text = "";
            txtRate.Text = "";
            txtTotal.Text = "";
            cboItem.Focus();

        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            calc();
        }
        private void calc()
        {
            decimal total = Convert.ToDecimal(txtQty.Text == "" ? "0" : txtQty.Text) * Convert.ToDecimal(txtRate.Text == "" ? "0" : txtRate.Text);
            txtTotal.Text = total.ToString();
        }

        private void txtRate_TextChanged(object sender, EventArgs e)
        {
            calc();
        }
        private void txtRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (cboItem.Text.Trim() == "" || cboItem.SelectedIndex < 0 || txtQty.Text.Trim() == "" || txtRate.Text.Trim() == "")
                {

                    MessageBox.Show("Please enter the details properly");
                    cboItem.Focus();
                }
                else
                {
                    string query = "";

                    double cgst = Convert.ToDouble(txtTotal.Text.Trim()) * 0.09;
                    double sgst = Convert.ToDouble(txtTotal.Text.Trim()) * 0.09;

                    query = "insert into Bill(Item_Code,Item,Qty,rate,total,cgstper,cgst,sgstper,sgst,igstper,igst,gsttotal) values('" + txtHSN.Text.Trim() + "','" + cboItem.SelectedValue + "'," + txtQty.Text.Trim() + "," + txtRate.Text.Trim() + "," + txtTotal.Text.Trim() + ",9,'" + cgst + "9,'" + sgst + ",0,0," + cgst + sgst + ")";

                    Connections.Instance.ExecuteQueries(query);
                  

                    query = "select sum(sgst),sum(cgst),sum(sgst)+sum(cgst) from Bill";
                    DataTable dt = (DataTable)Connections.Instance.ShowDataInGridView(query);

                    txtSGST.Text = dt.Rows[0][0].ToString();
                    txtCGST.Text = dt.Rows[0][1].ToString();
                    txtGST.Text = dt.Rows[0][3].ToString();

                    dt.Dispose();
                    cmdItemClear_Click(null, null);

                }
            }
        }
    }
}
