﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace VotingSystem
{
    public partial class EditStaffInformation : Form
    {
        public EditStaffInformation()
        {
            InitializeComponent();
        }

        string strcon, strsql;
        SqlConnection mycon;
        SqlCommand command;
        DataSet ds;
        SqlDataAdapter da;
        //Link database
        string Key;//Key word

        private bool DBConnect()
        {


            try
            {
                strcon = "Data Source=DESKTOP-BAERS9T\\SQLEXPRESS;Initial Catalog=Voting;Integrated Security=True";
                mycon = new SqlConnection(strcon);
                mycon.Open();

                MessageBox.Show("Link datebase is succesfully");
                return true;
            }
            catch
            {
                MessageBox.Show("Link datebase is not succesfully");
                return false;
            }
            //Check the database
        }

        private void showDataGrid()
        {
            strsql = "select * from VotingStaff";
            command = new SqlCommand(strsql, mycon);
            command.ExecuteScalar();
            ds = new DataSet();
            da = new SqlDataAdapter(command);
            da.Fill(ds, "votingstaff");
            StaffInfoGV.DataSource = ds.Tables["votingstaff"];
            //Show information to form
        }

        private void StaffInfoGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DBConnect();
            int currentline = e.RowIndex;
            if (e.ColumnIndex == StaffInfoGV.Columns["Delete"].Index)
            {
                string s = StaffInfoGV.CurrentRow.Cells[1].Value.ToString();
                DialogResult dr = MessageBox.Show("Are you sure to delete UserID=" + s, "inforamtion", MessageBoxButtons.OKCancel);
                if (dr == DialogResult.OK)
                {
                    strsql = string.Format("delete from VotingUsers where UserId ='{0}'", s);
                    MessageBox.Show(strsql);
                    try
                    {
                        command = new SqlCommand(strsql, mycon);
                        command.ExecuteScalar();
                        MessageBox.Show("delete successful");
                        showDataGrid();
                    }
                    catch
                    {
                        MessageBox.Show("delete sql statement error");
                    }

                    finally
                    {
                        mycon.Close();
                    }
                    //When click button "Del", it will delete information
                }
            }
        }

        private void StaffInfoGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Key = StaffInfoGV.CurrentRow.Cells[1].Value.ToString();
            Staff_txt.Text = StaffInfoGV.CurrentRow.Cells[1].Value.ToString();
            StaffName_txt.Text = StaffInfoGV.CurrentRow.Cells[2].Value.ToString();
            Password_txt.Text = StaffInfoGV.CurrentRow.Cells[3].Value.ToString();
            Role_cob.Text = StaffInfoGV.CurrentRow.Cells[4].Value.ToString();
            
        }

        private void OK_btn_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to update ID=" + Key, "inforamtion", MessageBoxButtons.OKCancel);
            //Show message "Are you sure to update"

            if (dr == DialogResult.OK)
            {
                DBConnect();
                strsql = string.Format("update VotingStaff set StaffName='{0}',Password='{1}',Role='{2}' where StaffId ='{3}'", StaffName_txt.Text, Password_txt.Text, Role_cob.Text, Key);
                MessageBox.Show(strsql);
                command = new SqlCommand(strsql, mycon);
                try
                {

                    command.ExecuteScalar();
                    MessageBox.Show("Successfully updated.");
                    showDataGrid();
                }
                catch
                {
                    MessageBox.Show("updated Error!!!!!!");
                }
                //Check update
                finally
                {
                    mycon.Close();
                }
                //Close database
            }
        }

        private void EditStaffInformation_Load(object sender, EventArgs e)
        {
            StaffName_txt.Select();
            DBConnect();
            showDataGrid();
            StaffInfoGV.RowHeadersVisible = false;
            this.StaffInfoGV.Columns[0].Width = 80;
            this.StaffInfoGV.Columns[1].Width = 80;
            this.StaffInfoGV.Columns[2].Width = 80;
            this.StaffInfoGV.Columns[3].Width = 80;
            StaffInfoGV.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 7, FontStyle.Bold);
        }
    }
}
