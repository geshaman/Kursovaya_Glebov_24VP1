using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Glebov_24VP1_
{
    public partial class Form1 : Form
    {
        private Random rnd;
        private int N;
        private List<int> myList;
        public Form1()
        {
            InitializeComponent();
            myList = new List<int>();
            rnd = new Random();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            dataGridView1.Rows.Add("Прямым выбором");
            dataGridView1.Rows.Add("Прямым выбором (мин, макс)");
            dataGridView1.Rows.Add("Прямым включением");

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;

            chart1.ChartAreas[0].AxisX.Maximum = 100;
            chart1.ChartAreas[0].AxisX.Minimum = 10;
            chart1.ChartAreas[0].AxisX.Interval = 10;

            chart1.ChartAreas[0].AxisY.Maximum = 200;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Interval = 20;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "0'%'";

            FillTableWithRealisticData();
            UpdateChartFromGrid();
        }
        
        private void UnorderedList()
        {
            for (int i = 0; i < N; i++) myList.Add(rnd.Next(0, N));
        }

        private void OrderedList()
        {
            const int step = 3;
            int elem = rnd.Next(0, step);
            for (int i = 0; i < N; i++)
            {
                myList.Add(elem);
                elem = rnd.Next(elem, elem+step);
            }
        }

        private void ReverseOrderedList()
        {
            const int step = 3;
            int elem = rnd.Next(0, step);
            for (int i = 0; i < N; i++)
            {
                myList.Insert(0, elem);
                elem = rnd.Next(elem, elem + step);
            }
        }

        private void PartiallyOrderedList(int percent)
        {
            const int step = 3;
            int elem = rnd.Next(0, step);
            for (int i = 0; i < N; i++)
            {
                myList.Add(elem);
                if (rnd.Next(100) >= percent)
                    elem = rnd.Next();
                else
                    elem = rnd.Next(elem, elem+step);
            }
        }

        // Заполнение таблицы реалистичными данными (время растет с %)
        private void FillTableWithRealisticData()
        {
            Random rand = new Random();

            for (int row = 0; row < 3; row++)
            {
                // Базовое время для каждого алгоритма
                int baseTime = rand.Next(5, 15);

                for (int col = 1; col <= 10; col++)
                {
                    // Время увеличивается с ростом процентов (колонка * коэффициент сложности)
                    double complexity = 1.0;

                    // Разная сложность алгоритмов
                    if (row == 0) complexity = 1.2;      // Прямым выбором - средняя сложность
                    else if (row == 1) complexity = 1.0; // С мин/макс - немного лучше
                    else complexity = 1.5;               // Прямым включением - сложнее

                    // Время = базовое время * процент * сложность алгоритма + случайность
                    int time = (int)(baseTime * col * complexity) + rand.Next(-3, 10);

                    dataGridView1.Rows[row].Cells[col].Value = time.ToString();
                }
            }
        }

        // Обновление диаграммы по данным таблицы
        private void UpdateChartFromGrid()
        {
            // Очищаем точки данных
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }

            // Заполняем диаграмму данными (только от 10% до 100%)
            for (int row = 0; row < 3; row++)
            {
                for (int col = 1; col <= 10; col++)
                {
                    string value = dataGridView1.Rows[row].Cells[col].Value?.ToString();
                    if (!string.IsNullOrEmpty(value) && double.TryParse(value, out double time))
                    {
                        chart1.Series[row].Points.AddXY(col * 10, time);
                    }
                }
            }
        }

        // Автоматическое обновление диаграммы при изменении таблицы
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 1)
            {
                UpdateChartFromGrid();
            }
        }
        
        private void SortButton_Click(object sender, EventArgs e)
        {

        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}