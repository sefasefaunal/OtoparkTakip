using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;
using OtoparkTakip;

namespace OtoparkTakip
{
    public partial class Form1 : Form
    {
        SqlConnection baglanti = new SqlConnection(@"Server=DESKTOP-Q6O3070;Database=OtoparkTakip;Trusted_Connection=True;TrustServerCertificate=True;");


        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AraclariListele();
        }

        void AraclariListele()
        {
            try
            {
                baglanti.Open();
                SqlDataAdapter da = new SqlDataAdapter("SELECT AracID, Plaka, Marka, Model, Renk, GirisTarihi FROM Araclar WHERE Durum=1", baglanti);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Listeleme hatasý: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void btnAracGiris_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPlaka.Text))
            {
                MessageBox.Show("Lütfen Plaka bilgisi girin.");
                return;
            }

            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("INSERT INTO Araclar (Plaka, Marka, Model, Renk, GirisTarihi, Durum) VALUES (@Plaka, @Marka, @Model, @Renk, @GirisTarihi, 1)", baglanti);
                komut.Parameters.AddWithValue("@Plaka", txtPlaka.Text.Trim());
                komut.Parameters.AddWithValue("@Marka", txtMarka.Text.Trim());
                komut.Parameters.AddWithValue("@Model", txtModel.Text.Trim());
                komut.Parameters.AddWithValue("@Renk", txtRenk.Text.Trim());
                komut.Parameters.AddWithValue("@GirisTarihi", DateTime.Now);

                komut.ExecuteNonQuery();
                MessageBox.Show("Araç giriþi kaydedildi.");
                AraclariListele();
                Temizle();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayýt hatasý: " + ex.Message);
            }
            finally
            {
                baglanti.Close();
            }
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            AraclariListele();
        }

        void Temizle()
        {
            txtPlaka.Clear();
            txtMarka.Clear();
            txtModel.Clear();
            txtRenk.Clear();
            txtPlaka.Focus();
        }

        private void btnAracGiris_Click_1(object sender, EventArgs e)
        {
            this.btnAracGiris.Click += new System.EventHandler(this.btnAracGiris_Click);
        }

        private void btnForm2_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2();
            frm2.Show(); // Yeni formu açar
            this.Hide(); // Formu gizler

        }
    }
}
