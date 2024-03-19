
namespace WindowsFormsApp
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSelectNode = new System.Windows.Forms.Button();
            this.drawEdgeButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.btnDrawNodes = new System.Windows.Forms.Button();
            this.deleteAllButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.findMaxFlowButton = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.graphEditor1 = new Graph.GraphEditor();
            this.SuspendLayout();
            // 
            // btnSelectNode
            // 
            this.btnSelectNode.Location = new System.Drawing.Point(629, 12);
            this.btnSelectNode.Name = "btnSelectNode";
            this.btnSelectNode.Size = new System.Drawing.Size(81, 37);
            this.btnSelectNode.TabIndex = 1;
            this.btnSelectNode.Text = "Выбрать вершину";
            this.btnSelectNode.UseVisualStyleBackColor = true;
            this.btnSelectNode.Click += new System.EventHandler(this.btnSelectNode_Click);
            // 
            // drawEdgeButton
            // 
            this.drawEdgeButton.Location = new System.Drawing.Point(629, 98);
            this.drawEdgeButton.Name = "drawEdgeButton";
            this.drawEdgeButton.Size = new System.Drawing.Size(81, 37);
            this.drawEdgeButton.TabIndex = 3;
            this.drawEdgeButton.Text = "Рисовать ребро";
            this.drawEdgeButton.UseVisualStyleBackColor = true;
            this.drawEdgeButton.Click += new System.EventHandler(this.drawEdgeButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(629, 141);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(81, 37);
            this.deleteButton.TabIndex = 4;
            this.deleteButton.Text = "Удалить элемент";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // btnDrawNodes
            // 
            this.btnDrawNodes.Location = new System.Drawing.Point(629, 55);
            this.btnDrawNodes.Name = "btnDrawNodes";
            this.btnDrawNodes.Size = new System.Drawing.Size(81, 37);
            this.btnDrawNodes.TabIndex = 5;
            this.btnDrawNodes.Text = "Рисовать вершину";
            this.btnDrawNodes.UseVisualStyleBackColor = true;
            this.btnDrawNodes.Click += new System.EventHandler(this.btnDrawNodes_Click);
            // 
            // deleteAllButton
            // 
            this.deleteAllButton.Location = new System.Drawing.Point(629, 184);
            this.deleteAllButton.Name = "deleteAllButton";
            this.deleteAllButton.Size = new System.Drawing.Size(81, 37);
            this.deleteAllButton.TabIndex = 6;
            this.deleteAllButton.Text = "Удалить все";
            this.deleteAllButton.UseVisualStyleBackColor = true;
            this.deleteAllButton.Click += new System.EventHandler(this.deleteAllButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(629, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(81, 37);
            this.button1.TabIndex = 8;
            this.button1.Text = "Выпуклость/вогнутость";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // findMaxFlowButton
            // 
            this.findMaxFlowButton.Location = new System.Drawing.Point(629, 270);
            this.findMaxFlowButton.Name = "findMaxFlowButton";
            this.findMaxFlowButton.Size = new System.Drawing.Size(142, 76);
            this.findMaxFlowButton.TabIndex = 9;
            this.findMaxFlowButton.Text = "Поиск максимальной пропускной способности";
            this.findMaxFlowButton.UseVisualStyleBackColor = true;
            this.findMaxFlowButton.Click += new System.EventHandler(this.findMaxFlowButton_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(629, 352);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(162, 95);
            this.listBox1.TabIndex = 11;
            // 
            // graphEditor1
            // 
            this.graphEditor1.DarkColor = System.Drawing.Color.DarkGray;
            this.graphEditor1.LightColor = System.Drawing.Color.DimGray;
            this.graphEditor1.Location = new System.Drawing.Point(13, 13);
            this.graphEditor1.Name = "graphEditor1";
            this.graphEditor1.ObjState = Graph.GraphEditor.ObjStates.osConvex;
            this.graphEditor1.Size = new System.Drawing.Size(610, 425);
            this.graphEditor1.TabIndex = 12;
            this.graphEditor1.Text = "graphEditor1";
            this.graphEditor1.EdgeAdd += new System.EventHandler(this.graphEditor1_EdgeAdd);
            this.graphEditor1.NodeAdd += new System.EventHandler(this.graphEditor1_NodeAdd);
            this.graphEditor1.NodeChoose += new System.EventHandler(this.graphEditor1_NodeChoose);
            this.graphEditor1.RemoveNode += new System.EventHandler(this.graphEditor1_RemoveNode);
            this.graphEditor1.RemoveEdge1 += new System.EventHandler(this.graphEditor1_RemoveEdge);
            this.graphEditor1.RemoveAll += new System.EventHandler(this.graphEditor1_RemoveAll);
            this.graphEditor1.ChangeState += new System.EventHandler(this.graphEditor1_ChangeState);
            this.graphEditor1.FindMaxFloww += new System.EventHandler(this.graphEditor1_FindMaxFlow);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 450);
            this.Controls.Add(this.graphEditor1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.findMaxFlowButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.deleteAllButton);
            this.Controls.Add(this.btnDrawNodes);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.drawEdgeButton);
            this.Controls.Add(this.btnSelectNode);
            this.MinimumSize = new System.Drawing.Size(790, 481);
            this.Name = "Form1";
            this.Text = "Редактор графов";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSelectNode;
        private System.Windows.Forms.Button drawEdgeButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button btnDrawNodes;
        private System.Windows.Forms.Button deleteAllButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button findMaxFlowButton;
        private System.Windows.Forms.ListBox listBox1;
        private Graph.GraphEditor graphEditor1;
    }
}

