using System.Text.RegularExpressions;
using System.Windows.Forms;
using System;

namespace OrganizationProfile
{
    public partial class frmRegistration : Form
    {
        
        private string _FullName;     
        private int _Age;             
        private long _ContactNo;      
        private long _StudentNo;      

        public frmRegistration()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                
                _FullName = FullName(txtLastName.Text, txtFirstName.Text, txtMiddleInitial.Text);
                string studentNoInput = txtStudentNo.Text;
                string contactNoInput = txtContactNo.Text;
                string program = cbPrograms.Text;
                string gender = cbGender.Text;
                DateTime birthdayDate = datePickerBirthday.Value;

                
                if (string.IsNullOrWhiteSpace(studentNoInput))
                {
                    throw new ArgumentNullException("Student number cannot be null or empty.");
                }

                if (!long.TryParse(studentNoInput, out _StudentNo))
                {
                    throw new FormatException("Student number must be a valid number.");
                }

                if (_StudentNo < 0)
                {
                    throw new OverflowException("Student number cannot be negative.");
                }

                if (!Regex.IsMatch(contactNoInput, @"^\+[0-9]{12}$"))
                {
                    throw new FormatException("Contact number must be in format +123456789.");
                }

                
                _ContactNo = long.Parse(contactNoInput);
               
                _Age = CalculateAge(birthdayDate);

                               if (!int.TryParse(txtAge.Text, out int enteredAge))
                {
                    throw new FormatException("Entered age must be a valid number.");
                }
              
                if (enteredAge != _Age)
                {
                    MessageBox.Show($"The entered age {enteredAge} does not match your birthday. Please check your inputs.", "Age Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; 
                }

                StudentInformationClass.SetFullName = _FullName;
                StudentInformationClass.SetStudentNo = (int)_StudentNo;
                StudentInformationClass.SetContactNo = _ContactNo;
                StudentInformationClass.SetProgram = program;
                StudentInformationClass.SetGender = gender;
                StudentInformationClass.SetBirthday = birthdayDate.ToString("yyyy-MM-dd");
                StudentInformationClass.SetAge = _Age; 

                
                frmConfirmation confirmationForm = new frmConfirmation();
                confirmationForm.Show();
                
                 
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Format Error: " + ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show("Argument Null Error: " + ex.Message);
            }
            catch (OverflowException ex)
            {
                MessageBox.Show("Overflow Error: " + ex.Message);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("Argument Error: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Execution completed.");
            }
        }

        public string FullName(string lastName, string firstName, string middleInitial)
        {
            if (Regex.IsMatch(lastName, @"^[a-zA-Z\s]+$") &&
               Regex.IsMatch(firstName, @"^[a-zA-Z\s]+$") &&
               Regex.IsMatch(middleInitial, @"^[a-zA-Z\s]*$")) 
            {
                return lastName + ", " + firstName + ", " + middleInitial;
            }

            throw new FormatException("Invalid name format.");
        }

        private int CalculateAge(DateTime birthDate)
        {
            int age = DateTime.Now.Year - birthDate.Year;
            if (birthDate > DateTime.Now.AddYears(-age)) age--;
            return age;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ListOfProgram = new string[]{
               "BS Information Technology",
               "BS Computer Science",
               "BS Information Systems",
               "BS in Accountancy",
               "BS in Hospitality Management",
               "BS in Tourism Management"
           };

            cbPrograms.Items.Clear();

            foreach (string program in ListOfProgram)
            {
                cbPrograms.Items.Add(program);
            }

            cbGender.Items.Clear(); 
            cbGender.Items.Add("Male");
            cbGender.Items.Add("Female");
        }
    }
}

