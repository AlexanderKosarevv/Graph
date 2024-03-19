using Graph;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class Form1 : Form
    {
        private GraphEditor graphEditor;
        public Form1()
        {
            InitializeComponent();
            graphEditor = new GraphEditor();
            graphEditor.Dock = DockStyle.None;
            this.Controls.Add(graphEditor);

            this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBox1.MeasureItem += lst_MeasureItem;
            this.listBox1.DrawItem += lst_DrawItem;
        }

        private void lst_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            // Устанавливаем высоту элемента списка (ItemHeight) на основе измерений графического объекта (Graphics)
            // Высота элемента определяется путем измерения строки текста, отображаемого в ListBox
            e.ItemHeight = (int)e.Graphics.MeasureString(listBox1.Items[e.Index].ToString(), listBox1.Font, listBox1.Width).Height;
        }

        private void lst_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Проверяем, есть ли элементы в ListBox
            if (listBox1.Items.Count > 0)
            {
                // Отрисовка фона элемента списка
                e.DrawBackground();
                // Отрисовка прямоугольника выделения элемента списка
                e.DrawFocusRectangle();
                // Отрисовка текста элемента списка
                // Используется графический объект (Graphics) для рисования текста с заданным шрифтом и цветом
                e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
            }
        }

        // Обработчик события нажатия на кнопку "Выбрать вершину"
        private void btnSelectNode_Click(object sender, EventArgs e)
        {
            // Включаем режим выбора вершин
            graphEditor1.SetSelectNodeMode(true);
        }

        // Обработчик события нажатия на кнопку "Рисовать вершину"
        private void btnDrawNodes_Click(object sender, EventArgs e)
        {
            // Включаем режим рисования вершин
            graphEditor1.SetDrawNodesMode(true);
        }

        // Обработчик события нажатия на кнопку "Рисовать ребро"
        private void drawEdgeButton_Click(object sender, EventArgs e)
        {
            // Включаем режим рисования ребер
            graphEditor1.SetDrawEdgeMode(true);
        }

        private void deleteAllButton_Click(object sender, EventArgs e)
        {
            // Метод удаления всех элеменов
            graphEditor1.DeleteAll();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            // Включаем режим удаления элементов
            graphEditor1.SetDeleteMode(true);
        }

        // Поменять выпуклость/вогнутость
        private void button1_Click(object sender, EventArgs e)
        {
            graphEditor1.State();
        }

        private void findMaxFlowButton_Click(object sender, EventArgs e)
        {
            // Включаем режим поиска максимальной пропускной способности
            graphEditor1.SetFindMaxFlowMode(true);
        }

        private void graphEditor1_EdgeAdd(object sender, EventArgs e)
        {
            listBox1.Items.Add("Добавлено ребро");
        }
        private void graphEditor1_NodeAdd(object sender, EventArgs e)
        {
            listBox1.Items.Add("Добавлена вершина");
        }
        private void graphEditor1_NodeChoose(object sender, EventArgs e)
        {
            listBox1.Items.Add("Выбрана вершина");
        }
        private void graphEditor1_RemoveNode(object sender, EventArgs e)
        {
            listBox1.Items.Add("Удалена вершина");
        }
        private void graphEditor1_RemoveEdge(object sender, EventArgs e)
        {
            listBox1.Items.Add("Удалено ребро");
        }
        private void graphEditor1_RemoveAll(object sender, EventArgs e)
        {
            listBox1.Items.Add("Удалено все");
        }
        private void graphEditor1_ChangeState(object sender, EventArgs e)
        {
            listBox1.Items.Add("Изменение состояния");
        }
        private void graphEditor1_FindMaxFlow(object sender, EventArgs e)
        {
            listBox1.Items.Add("Найдена максимальная пропускная способность");
        }
    }
}
