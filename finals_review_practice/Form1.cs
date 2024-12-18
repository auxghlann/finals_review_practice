using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.CodeDom;

namespace finals_review_practice
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbConnection conn;
        string connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"C:\\Users\\Khester Mesa\\Downloads\\dpPirates.accdb\"";


        int ID;
        //string pirate_name;
        //string name;
        //string pirate_group;
        int age;
        //double bounty;

        bool is_new_record;

        public void disable_controls()
        {
            txtAge.Enabled = false;
            txtAlias.Enabled = false;
            txtBounty.Enabled = false;
            txtName.Enabled = false;
            cboPirateGroup.Enabled = false;
        }

        public void enable_controls()
        {
            txtAge.Enabled = true;
            txtAlias.Enabled = true;
            txtBounty.Enabled = true;
            txtName.Enabled = true;
            cboPirateGroup.Enabled = true;
        }
        public void fill_cbo()
        {
            string query = "SELECT DISTINCT pirategroup FROM pirates";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
           
            cboSearchPirateGroup.DataSource = dt;
            cboSearchPirateGroup.DisplayMember = "pirategroup";
            cboSearchPirateGroup.ValueMember = "pirategroup";

            cboPirateGroup.DataSource = dt;
            cboPirateGroup.DisplayMember = "pirategroup";
            cboPirateGroup.ValueMember = "pirategroup";
            conn.Close();
        }

        public void fill_dataView()
        {
            string query = "SELECT ID, piratename as [ALIAS], givenname as [GIVEN NAME], age, pirategroup as [PIRATE GROUP], bounty as [BOUNTY] FROM pirates";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            conn.Close();
            grdResult.DataSource = dt;
            grdResult.Columns["ID"].Visible = false;
            grdResult.Columns["age"].Visible = false;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            fill_dataView();

            fill_cbo();

            btnSave.Enabled = false;
            disable_controls();
            is_new_record = false;

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string query = "SELECT ID, piratename as [ALIAS], givenname as [GIVEN NAME], age, pirategroup as [PIRATE GROUP], bounty as [BOUNTY] FROM pirates WHERE (piratename like @keyword or givenname like @keyword1) AND pirategroup = @group";
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@keyword", "%" + txtKeyword.Text + "%");
            cmd.Parameters.AddWithValue("@keyword1", "%" + txtKeyword.Text + "%");
            cmd.Parameters.AddWithValue("@group", cboSearchPirateGroup.Text);
            DataTable dt = new DataTable();

            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            adapter.Fill(dt);
            conn.Close();
            grdResult.DataSource = dt;
            grdResult.Columns["ID"].Visible = false;
            grdResult.Columns["age"].Visible = false;

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ID = Convert.ToInt32(grdResult.SelectedCells[0].Value.ToString()); // ID
            txtAlias.Text = grdResult.SelectedCells[1].Value.ToString(); // Alias
            txtName.Text = grdResult.SelectedCells[2].Value.ToString(); // Given Name
            txtAge.Text = grdResult.SelectedCells[3].Value.ToString(); // Age
            cboPirateGroup.Text = grdResult.SelectedCells[4].Value.ToString(); // Pirate Group
            txtBounty.Text = grdResult.SelectedCells[5].Value.ToString(); // Bounty
        }

        private void btnNewRecord_Click(object sender, EventArgs e)
        {
            btnNewRecord.Enabled = false;

            txtAge.Clear();
            txtAlias.Clear();
            txtName.Clear();
            txtBounty.Clear();
            cboPirateGroup.SelectedIndex = -1;

            btnSave.Enabled = true;
            enable_controls();
            is_new_record = true;


            //string query = "INSERT  ID, piratename as [ALIAS], givenname as [GIVEN NAME], age, pirategroup as [PIRATE GROUP], bounty as [BOUNTY] FROM pirates WHERE (piratename like @keyword or givenname like @keyword1) AND pirategroup = @group";
            //conn = new OleDbConnection(connStr);
            //conn.Open();
            //OleDbCommand cmd = new OleDbCommand(query, conn);
            //cmd.Parameters.AddWithValue("@keyword", "%" + txtKeyword.Text + "%");
            //cmd.Parameters.AddWithValue("@keyword1", "%" + txtKeyword.Text + "%");
            //cmd.Parameters.AddWithValue("@group", cboSearchPirateGroup.Text);
            //DataTable dt = new DataTable();

            //OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            //adapter.Fill(dt);
            //conn.Close();
            //grdResult.DataSource = dt;
            //grdResult.Columns["ID"].Visible = false;
            //grdResult.Columns["age"].Visible = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (is_new_record == true)
            {
                string query = "INSERT INTO pirates (piratename, givenname, age, pirategroup, bounty) VALUES (@piratename, @givenname, @age, @pirategroup, @bounty)";
                conn = new OleDbConnection(connStr);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@piratename", txtAlias.Text);
                cmd.Parameters.AddWithValue("@givenname", txtName.Text);
                cmd.Parameters.AddWithValue("@age", Convert.ToInt32(txtAge.Text));
                cmd.Parameters.AddWithValue("@pirategroup", cboPirateGroup.Text);
                cmd.Parameters.AddWithValue("@bounty", Convert.ToDouble(txtBounty.Text));
                cmd.ExecuteNonQuery();
                conn.Close();
            } else
            {
                string query = "UPDATE pirates SET piratename = @piratename, givenname = @givenname, age = @age, pirategroup = @pirategroup, bounty = @bounty WHERE ID = @id";
                conn = new OleDbConnection(connStr);
                conn.Open();
                OleDbCommand cmd = new OleDbCommand(query, conn);
                cmd.Parameters.AddWithValue("@piratename", txtAlias.Text);
                cmd.Parameters.AddWithValue("@givenname", txtName.Text);
                cmd.Parameters.AddWithValue("@age", Convert.ToInt32(txtAge.Text));
                cmd.Parameters.AddWithValue("@pirategroup", cboPirateGroup.Text);
                cmd.Parameters.AddWithValue("@bounty", Convert.ToDouble(txtBounty.Text));
                cmd.Parameters.AddWithValue("@id", ID);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            

            fill_dataView();

            btnSave.Enabled = false;
            disable_controls();
            btnNewRecord.Enabled = true;
            is_new_record = false;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            string query = "SELECT ID, piratename as [ALIAS], givenname as [GIVEN NAME], age, pirategroup as [PIRATE GROUP], bounty as [BOUNTY] FROM pirates where ID = @id";
            
            conn = new OleDbConnection(connStr);
            conn.Open();
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", Convert.ToInt32(ID));
            DataTable dt = new DataTable();
            OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
            adapter.Fill(dt);
            grdResult.DataSource = dt;
            conn.Close();

            enable_controls();
            btnNewRecord.Enabled = false;
            btnSave.Enabled = true;
        }
    }
}
