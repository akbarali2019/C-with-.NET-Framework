using Google.Cloud.Firestore.V1;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Cloud.Firestore;
using System.Collections;
using Google.Type;
using Newtonsoft.Json.Linq;

namespace CloudFireEng
{
    
    public partial class Form1 : Form
    {
        FirestoreDb database;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"cloudfire.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            database = FirestoreDb.Create("cloudfire-a0cd4");
            //MessageBox.Show("successful");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Add_Document_with_AutoID();
            //Add_Document_with_CustomID();
            //Add_Array();
            Add_List();
        }

        void Add_Document_with_AutoID()
        {
            CollectionReference coll = database.Collection("Add_Document_with_AutoID");
            Dictionary<string, object> data1 = new Dictionary<string, object>()
            {
                {"FirstName", "Bobur" },
                {"LastName", "Boburxanov" },
                {"PhoneNumber", 123456789 }
            };
            coll.AddAsync(data1);
            MessageBox.Show("Added Successfully");
        }

        void Add_Document_with_CustomID()
        {
            DocumentReference doc = database.Collection("Add_Document_with_CustomID").Document("firstDoc");
            Dictionary<string, object> data1 = new Dictionary<string, object>()
            {
                {"FirstName", "Bobur" },
                {"LastName", "Boburxanov" },
                {"PhoneNumber", 123456789 }
            };
            doc.SetAsync(data1);
            MessageBox.Show("Added Successfully");
        }

        void Add_Array()
        {
            DocumentReference doc = database.Collection("Add_Array").Document("firstArray");
            Dictionary<string, object> data1 = new Dictionary<string, object>();

            ArrayList myArray = new ArrayList();
            myArray.Add(123);
            myArray.Add("name");
            myArray.Add(true);

            data1.Add("myArray", myArray);

            doc.SetAsync(data1);
            MessageBox.Show("Added Successfully");
        }

        void Add_List()
        {
            DocumentReference doc = database.Collection("Add_List").Document("firstList");
          
            Dictionary<string, object> myList = new Dictionary<string, object>();
            

            Dictionary<string, object> list1 = new Dictionary<string, object>()
            {
                {"FirstName", "Bobur" },
                {"LastName", "Boburxanov" },
                {"PhoneNumber", 123456789 },
            };

            myList.Add("myList", list1);
            

            doc.SetAsync(myList);
            
            MessageBox.Show("Added Successfully");
        }
        
    }
}
