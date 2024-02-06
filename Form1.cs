using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business_Layer;

namespace Blood_Bank
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private enum  enPage : byte
        {
            DahsBoard,
            Donors,
            AddNewDonor,
            UpdateDonor,
            DonateHistory,
            patients,
            AddNewPatient,
            UpdatePatient,
            Donate,
            BloodStock,
            BloodTransfer,
            BloodTransferHistory,
        }

        string BloodGroupName = "";
        string Gender = "";


        private void _RefreshDonorsGridView()
        {
            dataGridView1.DataSource = Donor.GetAllDonors();
        }

        private void _RefreshPatientsGridView()
        {
            dtPatientsTable.DataSource = Patient.GetAllPatients();
        }


        private void _RefreshBloodStockGridView()
        {
            dgvBloodStock.DataSource = BloodStock.GetAllBloodNameAndQuantityInStock();
        }

        private void _RefreshDonateHistoryGridView()
        {
            dgvDonateHistoryList.DataSource = DonateHistroy.GetAllDonateHistroy();
        }

        private void _AddNewRecordInDonateHistory()
        {
            DonateHistroy DonateHis = new DonateHistroy();

            DonateHis.DonorID = Convert.ToInt32(dgvDonorsDonatePage.CurrentRow.Cells[0].Value);

            DonateHis.Save();
        }

        private void _AddNewRecordInTransferHistory()
        {
            TransferHistory TransferHis = new TransferHistory();

            TransferHis.PatientID = Convert.ToInt32(dgvPatientList_TransferBlood.CurrentRow.Cells[0].Value);

            TransferHis.Save();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDonors_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.Donors;
            _RefreshDonorsGridView();

        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.DahsBoard;
        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.patients;
            _RefreshPatientsGridView();
        }

        private void btnDonate_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.Donate;
            dgvDonorsDonatePage.DataSource = Donor.GetAllDonors();
        }

        private void btnBloodStock_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.BloodStock;
            _RefreshBloodStockGridView();
        }

        private void btnBloodTransfer_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.BloodTransfer;
            dgvPatientList_TransferBlood.DataSource = Patient.GetAllPatients();

        }

        private void tpDonors_Click(object sender, EventArgs e)
        {

        }

        private void txtDonorsSearch_TextChanged(object sender, EventArgs e)
        {

            if (txtDonorsSearch.Text == "")
            {
                _RefreshDonorsGridView();
            }

            else
            {
                dataGridView1.DataSource = Donor.GetAllDonorsByID(txtDonorsSearch.Text);
            }
        }

        private void gunaComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbBloodGroupDonor.SelectedItem == "All" || cbBloodGroupDonor.SelectedItem == null)
            {
                _RefreshDonorsGridView();
            }

            else
            {
                dataGridView1.DataSource = Donor.FilterByBloodGroupName(cbBloodGroupDonor.SelectedItem.ToString());
            }
        }

        private void gunaTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddNewDonor_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.AddNewDonor;
        }

        private void btnDonorSave_Click(object sender, EventArgs e)
        {
            Person AddPerson = new Person();

            AddPerson.FistName = txtDonorFirstName.Text;
            AddPerson.LastName = txtDonorLastName.Text;
            AddPerson.PhoneNumber = txtDonorPhoneNumber.Text;
            AddPerson.Address = txtDonorAddress.Text;
            AddPerson.Age = Convert.ToInt32(txtDonorAge.Text);
            AddPerson.Gender = Gender;


            if(AddPerson.Save())
            {
                Donor AddDonor = new Donor();
                BloodGroup Blood = BloodGroup.FindByBloodGroupName(BloodGroupName);

                AddDonor.PersonID = AddPerson.PersonID;
                AddDonor.BloodID = Blood.BloodID;

                if(AddDonor.Save())
                {
                    txtDonorID.Text = AddDonor.DonorID.ToString();
                    MessageBox.Show("Add Donor Successfully", "Add Donor");
                }

            }
            _RefreshDonorsGridView();

        }

        private void btnDonorBack_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.Donors;
            _RefreshDonorsGridView();
        }

        private void cbDonorBloodGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            BloodGroupName = cbDonorBloodGroup.SelectedItem.ToString();
        }

        private void btnDonorMale_Click(object sender, EventArgs e)
        {
            Gender = "Male";
        }

        private void btnDonorFemale_Click(object sender, EventArgs e)
        {
            Gender = "Female";
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.UpdateDonor;
            int DonorID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            Donor Donor1 = Donor.Find(DonorID);
            Person Person1 = Person.Find(Donor1.PersonID);


            txtUpdateDonorID.Text = Donor1.DonorID.ToString();
            txtUpdateDonorFName.Text = Person1.FistName;
            txtUpdateDonorLName.Text = Person1.LastName;
            txtUpdateDonorAge.Text = Person1.Age.ToString();
            txtUpdateDonorPhone.Text = Person1.PhoneNumber;
            txtDonorUpdateAddress.Text = Person1.Address;

            //cbCountries.SelectedIndex = cbCountries.FindString(Country.Find(_Contact.CountryID).CountryName);
            cbUpdateDonorBloodGroup.SelectedIndex = cbUpdateDonorBloodGroup.FindString(BloodGroup.Find(Donor1.BloodID).BloodGroupName);

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Delete Donor With ID: " + dataGridView1.CurrentRow.Cells[0].Value, "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {

                int DonorID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

                if (Donor.DeleteDonor(DonorID))
                {
                    MessageBox.Show("Donor Deleted Successfully");
                    _RefreshDonorsGridView();
                }

                else
                    MessageBox.Show("Donor Delete Faild");
            }
        }

        private void btnUpdateDonorBack_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.Donors;
            _RefreshDonorsGridView();
        }

        private void btnUpdateDonor_Click(object sender, EventArgs e)
        {
            Donor Donor1 = Donor.Find(Convert.ToInt32(txtUpdateDonorID.Text));
            Person Person1 = Person.Find(Convert.ToInt32(Donor1.PersonID));

            BloodGroup Blood = BloodGroup.FindByBloodGroupName(BloodGroupName);

            Person1.FistName = txtUpdateDonorFName.Text;
            Person1.LastName = txtUpdateDonorLName.Text;
            Person1.Age = Convert.ToInt32(txtUpdateDonorAge.Text);
            Person1.PhoneNumber = txtUpdateDonorPhone.Text;
            Person1.Address = txtDonorUpdateAddress.Text;
            Person1.Gender = Gender;
            Donor1.BloodID = Blood.BloodID;

            if(Person1.Save())
            {

                if (Donor1.Save())
                {
                    MessageBox.Show("Donor Updated Successfully");
                    _RefreshDonorsGridView();
                }

                else
                {
                    MessageBox.Show("Update Donor Failed");
                }

            }

                        
        }

        private void cbUpdateDonorBloodGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            BloodGroupName = cbUpdateDonorBloodGroup.SelectedItem.ToString();

        }

        private void btnUpdateDonorMale_Click(object sender, EventArgs e)
        {
            Gender = "Male";
        }

        private void btnUpdateDonorFemale_Click(object sender, EventArgs e)
        {
            Gender = "Female";
        }

        private void Patients_Click(object sender, EventArgs e)
        {

        }

        private void txtSearchPatient_TextChanged(object sender, EventArgs e)
        {
            if (txtSearchPatient.Text == "")
            {
                _RefreshPatientsGridView();
            }

            else
            {
                dtPatientsTable.DataSource = Patient.GetAllPatientsByID(txtSearchPatient.Text);
            }
        }

        private void btnAddNewPatien_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.AddNewPatient;

        }

        private void btnPatinetSave_Click(object sender, EventArgs e)
        {
            Person AddPerson = new Person();

            AddPerson.FistName = txtPatietnFirstName.Text;
            AddPerson.LastName = txtPatietnLastName.Text;
            AddPerson.PhoneNumber = txtPatietnPhoneNumber.Text;
            AddPerson.Address = txtPatietnAddress.Text;
            AddPerson.Age = Convert.ToInt32(txtPatietnAge.Text);
            AddPerson.Gender = Gender;


            if (AddPerson.Save())
            {
                Patient AddPatient = new Patient();
                BloodGroup Blood = BloodGroup.FindByBloodGroupName(BloodGroupName);

                AddPatient.PersonID = AddPerson.PersonID;
                AddPatient.BloodID = Blood.BloodID;

                if (AddPatient.Save())
                {
                    txtPatientID.Text = AddPatient.PatientID.ToString();
                    MessageBox.Show("Add Patient Successfully", "Add Patient");
                }

            }
            _RefreshPatientsGridView();
        }

        private void cbPatientBlood_SelectedIndexChanged(object sender, EventArgs e)
        {
            BloodGroupName = cbPatientBlood.SelectedItem.ToString();
        }

        private void cbPatinetMale_Click(object sender, EventArgs e)
        {
            Gender = "Male";
        }

        private void btnPatientFemale_Click(object sender, EventArgs e)
        {
            Gender = "Female";
        }

        private void btnPatientBack_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.patients;
        }

        private void cbPatientBloodGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPatientBloodGroup.SelectedItem == "All" || cbPatientBloodGroup.SelectedItem == null)
            {
                _RefreshPatientsGridView();
            }

            else
            {
                dtPatientsTable.DataSource = Patient.GetAllPatientsByBloodGroup(cbPatientBloodGroup.SelectedItem.ToString());
            }
        }

        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.UpdatePatient;
            int PatientID = Convert.ToInt32(dtPatientsTable.CurrentRow.Cells[0].Value);

            Patient Patient = Patient.Find(PatientID);
            Person Person1 = Person.Find(Patient.PersonID);

            
            txtUpdatePatientID.Text = Patient.PatientID.ToString();
            txtUpdatePatientFName.Text = Person1.FistName;
            txtUpdatePatientLName.Text = Person1.LastName;
            txtUpdatePatientAge.Text = Person1.Age.ToString();
            txtUpdatePatientPhone.Text = Person1.PhoneNumber;
            txtUpdatePatientAddress.Text = Person1.Address;

            cbUpdatePatientBloodGroup.SelectedIndex = cbUpdatePatientBloodGroup.FindString(BloodGroup.Find(Patient.BloodID).BloodGroupName);
        }

        private void btnUpdatePatientBack_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.patients;
            _RefreshPatientsGridView();


        }

        private void btnUpdatePatient_Click(object sender, EventArgs e)
        {
            Patient Patient1 = Patient.Find(Convert.ToInt32(txtUpdatePatientID.Text));
            Person Person1 = Person.Find(Convert.ToInt32(Patient1.PersonID));

            BloodGroup Blood = BloodGroup.FindByBloodGroupName(BloodGroupName);

            Person1.FistName = txtUpdatePatientFName.Text;
            Person1.LastName = txtUpdatePatientLName.Text;
            Person1.Age = Convert.ToInt32(txtUpdatePatientAge.Text);
            Person1.PhoneNumber = txtUpdatePatientPhone.Text;
            Person1.Address = txtUpdatePatientAddress.Text;
            Person1.Gender = Gender;
            Patient1.BloodID = Blood.BloodID;

            if (Person1.Save())
            {

                if (Patient1.Save())
                {
                    MessageBox.Show("Patient Updated Successfully");
                    _RefreshPatientsGridView();
                }

                else
                {
                    MessageBox.Show("Update Patient Failed");
                }

            }
        }

        private void cbUpdatePatientBloodGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            BloodGroupName = cbUpdatePatientBloodGroup.SelectedItem.ToString();

        }

        private void btnUpdatePatientMale_Click(object sender, EventArgs e)
        {
            Gender = "Male";
        }

        private void btnUpdatePatientFemale_Click(object sender, EventArgs e)
        {
            Gender = "Female";

        }

        private void dgvDonorsDonatePage_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvBloodStockDonate.DataSource = BloodStock.GetBloodStockNameAndQuantityByBloodName(dgvDonorsDonatePage.CurrentRow.Cells[7].Value.ToString());
            lblDonorFullName.Text = dgvDonorsDonatePage.CurrentRow.Cells[1].Value.ToString() + " " + dgvDonorsDonatePage.CurrentRow.Cells[2].Value.ToString();
            lblDonorBloodGroup.Text = dgvDonorsDonatePage.CurrentRow.Cells[7].Value.ToString();
        }

        private void tpDonate_Click(object sender, EventArgs e)
        {
            
        }

        private void btnDonateBlood_Click(object sender, EventArgs e)
        {
            BloodGroup Blood = BloodGroup.FindByBloodGroupName(lblDonorBloodGroup.Text);
            BloodStock Bloodstock = BloodStock.FindByBloodID(Blood.BloodID);
            Bloodstock.QuantityInStock += 1; 

            if(Bloodstock.Save())
            {
                MessageBox.Show("Donate Success");
                dgvBloodStockDonate.DataSource = BloodStock.GetBloodStockNameAndQuantityByBloodName(dgvDonorsDonatePage.CurrentRow.Cells[7].Value.ToString());
                _RefreshBloodStockGridView();
                _AddNewRecordInDonateHistory();

            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gunaButton1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.DonateHistory;
            _RefreshDonateHistoryGridView();

        }

        private void btnDonateHistoryBack_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = (byte)enPage.Donate;
        }

        private void dgvPatientList_TransferBlood_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void transferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvBloodStockTransfer.DataSource = BloodStock.GetBloodStockNameAndQuantityByBloodName(dgvPatientList_TransferBlood.CurrentRow.Cells[7].Value.ToString());
            lblPatientName.Text = dgvPatientList_TransferBlood.CurrentRow.Cells[1].Value.ToString() + " " + dgvPatientList_TransferBlood.CurrentRow.Cells[2].Value.ToString();
            lblPatinetBloodGroup.Text = dgvPatientList_TransferBlood.CurrentRow.Cells[7].Value.ToString();
        }

        private void btnTransferBlood_Click(object sender, EventArgs e)
        {

            //if (dgvBloodStockTransfer.Rows[0].Cells[1].Value == null || Convert.ToInt16(dgvBloodStockTransfer.Rows[0].Cells[1].Value) == 0)
            //{
            //    btnTransferBlood.Enabled = false;
            //    //return;
            //}
            //else
            //{
            //    btnTransferBlood.Enabled = true;
            //}

            int NumberOfBloodInStock = Convert.ToInt32(dgvBloodStockTransfer.Rows[0].Cells[1].Value);

            if(NumberOfBloodInStock > 0 && lblPatientName.Text != "")
            {
                BloodGroup Blood = BloodGroup.FindByBloodGroupName(lblPatinetBloodGroup.Text);
                BloodStock Bloodstock = BloodStock.FindByBloodID(Blood.BloodID);
                Bloodstock.QuantityInStock -= 1;


                if (Bloodstock.Save())
                {
                    MessageBox.Show("Transfer Done");
                    dgvBloodStockTransfer.DataSource = BloodStock.GetBloodStockNameAndQuantityByBloodName(dgvPatientList_TransferBlood.CurrentRow.Cells[7].Value.ToString());
                    _RefreshBloodStockGridView();
                    _AddNewRecordInTransferHistory();

                }

            }

            else
            {
                MessageBox.Show("Select Patient First And \nThe Quantity Should Be More Than 0");

            }
        }

        private void tpBloodTransfer_Click(object sender, EventArgs e)
        {

        }
    }
}
