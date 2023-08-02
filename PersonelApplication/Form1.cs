using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace PersonelApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnsave_Click(object sender, EventArgs e)
        {

            await setPersonel();

        }


        private async void btngetir_Click(object sender, EventArgs e)
        {

            await getPersonel(); 
           
        }
        private async Task setPersonel()
        {
            Personel personel = new Personel();
            personel.Ad = txtad.Text;
            personel.Soyad = txtsoyad.Text;

            var Client = new HttpClient();
            Client.BaseAddress = new Uri("http://localhost:80");

            JavaScriptSerializer js = new JavaScriptSerializer();
            var SerializedPersonel = js.Serialize(personel);

            StringContent strcontent = new StringContent(SerializedPersonel, Encoding.UTF8, "application/json");

            try
            {
                await Client.PostAsync("api/Personel/insertpersonel", strcontent);
                MessageBox.Show("Kayıt Eklendi.");
                TextboxClear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata");
            }

        }
        private async Task getPersonel()
        {
            List<Personel> list = new List<Personel>();
            var Client = new HttpClient();

            Client.BaseAddress = new Uri("http://localhost:80");

            try
            {
                var response = await Client.GetAsync("/api/getPersonel/selectpersonel");
                var jsonString = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<List<Personel>>(jsonString);

                labelSet(list);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "hata");
            }
        }
        private void labelSet(List<Personel> list)
        {
            lblad.Text = list[0].Ad;
            lblid.Text = list[0].ID.ToString();
            lblsoyad.Text = list[0].Soyad;
        }
        private void TextboxClear()
        {
            txtad.Clear();
            txtsoyad.Clear();
        }

    }
}
