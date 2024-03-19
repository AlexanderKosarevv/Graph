using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Graph
{
    public class GraphEditor : Control
    {
        protected Color _DarkColor;
        protected Color _LightColor;
        private int nodeSize = 30; // Размер вершин
        private List<string> edgeWeights = new List<string>(); // Коллекция для хранения весов ребер
        private List<Node> nodes; // Список вершин
        private List<Tuple<Point, Point>> edges; // Список ребер
        private bool drawNodesMode; // Флаг режима рисования вершин
        private bool selectNodeMode; // Флаг режима выбора вершин
        private bool drawEdgeMode; // Флаг режима рисования ребра
        private bool deleteMode; // Флаг режима удаления элементов
        private bool findMaxFlowMode; // Флаг режима поиска максимальной пропускной способности
        private int selectedNodeIndex = -1; // Инициализация переменной selectedNodeIndex
        private int startNodeIndex = -1; // Инициализация переменной startNodeIndex
        private int selectedEdgeIndex = -1; // Индекс выбранного ребра
        private int firstSelectedNodeIndex; // Индекс первой выбранной вершины для поиска максимальной пропускной способности
        public GraphEditor() : base()
        {
            nodes = new List<Node>();
            edges = new List<Tuple<Point, Point>>();
            // Настройки внешнего вида
            DoubleBuffered = true; // убирает мерцание
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, true);
            TabStop = true;
            // Обработчик события клика мыши
            MouseDown += GraphEditor_MouseDown;
            _LightColor = Color.DimGray;
            _DarkColor = Color.DarkGray;
        }

        // Метод для включения/выключения режима рисования вершин
        public void SetDrawNodesMode(bool mode)
        {
            drawNodesMode = mode;
            selectNodeMode = false; // Выключаем режим выбора вершин
            drawEdgeMode = false; // Выключаем режим рисования ребра
            deleteMode = false; // Выключаем режим удаления элемента
            findMaxFlowMode = false; // Выключаем режим поиска максимальной пропускной способности
            selectedNodeIndex = -1; // Сбрасываем выбранную вершину
            startNodeIndex = -1; // Сбрасываем начальную вершину
            firstSelectedNodeIndex = -1; // Сбрасываем первую вершину
            selectedEdgeIndex = -1; // Сбрасываем индекс выбранного ребра
        }

        // Метод для включения/выключения режима выбора вершин
        public void SetSelectNodeMode(bool mode)
        {
            selectNodeMode = mode;
            drawNodesMode = false; // Выключаем режим рисования вершин
            drawEdgeMode = false; // Выключаем режим рисования ребра
            deleteMode = false; // Выключаем режим удаления элемента
            findMaxFlowMode = false; // Выключаем режим поиска максимальной пропускной способности
            selectedNodeIndex = -1; // Сбрасываем выбранную вершину
            startNodeIndex = -1; // Сбрасываем начальную вершину
        }

        // Метод для включения/выключения режима рисования ребра
        public void SetDrawEdgeMode(bool mode)
        {
            drawEdgeMode = mode;
            drawNodesMode = false; // Выключаем режим рисования вершин
            selectNodeMode = false; // Выключаем режим выбора вершин
            deleteMode = false; // Выключаем режим удаления элемента
            findMaxFlowMode = false; // Выключаем режим поиска максимальной пропускной способности
            selectedNodeIndex = -1; // Сбрасываем выбранную вершину
            startNodeIndex = -1; // Сбрасываем начальную вершину
        }

        // Метод для включения/выключения режима удаления элементов
        public void SetDeleteMode(bool mode)
        {
            deleteMode = mode;
            drawNodesMode = false; // Выключаем режим рисования вершин
            selectNodeMode = false; // Выключаем режим выбора вершин
            drawEdgeMode = false; // Выключаем режим рисования ребра
            findMaxFlowMode = false; // Выключаем режим поиска максимальной пропускной способности
            selectedNodeIndex = -1; // Сбрасываем выбранную вершину
            selectedEdgeIndex = -1; // Сбрасываем выбранное ребро
        }

        // Метод для включения/выключения режима поиска максимальной пропускной способности
        public void SetFindMaxFlowMode(bool mode)
        {
            findMaxFlowMode = mode; // Включаем/выключаем режим поиска максимальной пропускной способности
            drawNodesMode = false; // Выключаем режим рисования вершин
            selectNodeMode = false; // Выключаем режим выбора вершин
            drawEdgeMode = false; // Выключаем режим рисования ребра
            deleteMode = false; // Выключаем режим удаления элемента
            selectedNodeIndex = -1; // Сбрасываем выбранную вершину
            startNodeIndex = -1; // Сбрасываем начальную вершину
        }

        // Метод для удаления всех вершин и ребер
        public void DeleteAll()
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить все вершины и ребра?", "Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                nodes.Clear(); // очищаем вершины
                edges.Clear(); // очищаем ребра
                edgeWeights.Clear(); // очищаем веса ребер
                selectedNodeIndex = -1;
                startNodeIndex = -1;
                OnRemoveAll();
                Invalidate(); // Перерисовываем компонент
            }
        }

        // Переопределяет реализацию базового класса для установки границ компонента контрола.
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            const int minWidth = 100; // Минимальная ширина компонента
            const int minHeight = 100; // Минимальная высота 
            // Применяем ограничения на ширину и высоту
            width = Math.Max(width, minWidth);
            height = Math.Max(height, minHeight);
            base.SetBoundsCore(x, y, width, height, specified);
        }

        // Обрабатывает событие MouseDown, которое возникает при нажатии кнопки мыши в области компонента GraphEditor.
        private void GraphEditor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (deleteMode)
                {
                    // Проверяем, попали ли мы в какую-либо вершину
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        Rectangle nodeBounds = new Rectangle(nodes[i].Point.X - 10, nodes[i].Point.Y - 10, 20, 20);
                        if (nodeBounds.Contains(e.Location))
                        {
                            // Подсвечиваем выбранный элемент
                            selectedNodeIndex = i;
                            Invalidate(); // Перерисовываем компонент
                            // Отображаем предупреждение об удалении
                            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту вершину и связанные с ней ребра?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes)
                            {
                                // Удаляем вершину и связанные ребра
                                RemoveNodeAndEdges(i);
                                OnRemoveNode();
                            }
                            return;
                        }
                    }
                    // Проверяем, попали ли мы в какое-либо ребро
                    for (int i = 0; i < edges.Count; i++)
                    {
                        Tuple<Point, Point> edge = edges[i];
                        if (IsPointOnEdge(e.Location, edge.Item1, edge.Item2))
                        {
                            // Подсвечиваем выбранное ребро
                            selectedEdgeIndex = i;
                            Invalidate(); // Перерисовываем компонент
                            // Отображаем предупреждение об удалении
                            DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить это ребро?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == DialogResult.Yes)
                            {
                                // Удаляем ребро
                                RemoveEdge(i);
                                OnRemoveEdge();
                            }
                            return;
                        }
                    }
                }
                else if (selectNodeMode)
                {
                    // Проверяем, попали ли мы в какую-либо вершину
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        Rectangle nodeBounds = new Rectangle(nodes[i].Point.X - 10, nodes[i].Point.Y - 10, 20, 20);
                        if (nodeBounds.Contains(e.Location))
                        {
                            // Выбираем вершину и меняем ее цвет
                            selectedNodeIndex = i;
                            OnNodeChoose();
                            Invalidate(); // Перерисовываем компонент
                            return;
                        }
                    }
                }
                else if (drawNodesMode)
                {
                    Point clickPoint = e.Location;
                    // Проверяем, чтобы координаты клика не выходили за пределы GraphEditor
                    if (clickPoint.X < 30 || clickPoint.X >= Width - 30 || clickPoint.Y < 30 || clickPoint.Y >= Height - 30)
                    {
                        return;
                    }
                    AddNode(clickPoint);
                }
                else if (drawEdgeMode)
                {
                    // Проверяем, попали ли мы в какую-либо вершину
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        Rectangle nodeBounds = new Rectangle(nodes[i].Point.X - 10, nodes[i].Point.Y - 10, 20, 20);
                        if (nodeBounds.Contains(e.Location))
                        {
                            // Проверяем, является ли это начальной вершиной
                            if (startNodeIndex == -1)
                            {
                                // Выбираем начальную вершину и меняем ее цвет
                                startNodeIndex = i;
                                selectedNodeIndex = i;
                                Invalidate(); // Перерисовываем компонент
                            }
                            else
                            {
                                // Создаем ребро между начальной и конечной вершинами
                                Point start = nodes[startNodeIndex].Point;
                                Point end = nodes[i].Point;
                                AddEdge(start, end);
                                // Сбрасываем начальную вершину
                                startNodeIndex = -1;
                                selectedNodeIndex = -1;
                                Invalidate(); // Перерисовываем компонент
                            }
                            return;
                        }
                    }
                }
                else if (findMaxFlowMode)
                {
                    // Проверяем, попали ли мы в какую-либо вершину
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        Rectangle nodeBounds = new Rectangle(nodes[i].Point.X - 10, nodes[i].Point.Y - 10, 20, 20);
                        if (nodeBounds.Contains(e.Location))
                        {
                            if (firstSelectedNodeIndex == -1)
                            {
                                // Выбираем первую вершину
                                firstSelectedNodeIndex = i;
                                nodes[i].Color = Color.Red; // Изменяем цвет вершины
                                Invalidate(); // Перерисовываем компонент
                            }
                            else
                            {
                                // Проверяем, существует ли путь от первой вершины к текущей
                                bool pathExists = CheckPathExists(firstSelectedNodeIndex, i);
                                if (pathExists)
                                {
                                    // Вычисляем максимальную пропускную способность
                                    int maxFlow = FindMaxFlow(firstSelectedNodeIndex, i);
                                    MessageBox.Show($"Максимальная пропускная способность: {maxFlow}", "Результат");
                                    OnFindMaxFlow();
                                }
                                else
                                {
                                    MessageBox.Show("Невозможно найти путь от первой вершины ко второй", "Ошибка");
                                }
                                // Сбрасываем выбор вершин и цвета
                                firstSelectedNodeIndex = -1;
                                foreach (var node in nodes)
                                {
                                    node.Color = Color.Black;
                                }
                                Invalidate(); // Перерисовываем компонент
                            }
                            return;
                        }
                    }
                }
            }
        }

        // Выполняет поиск пути между двумя вершинами в графе
        private bool CheckPathExists(int startNodeIndex, int endNodeIndex)
        {
            // Инициализируем массив посещенных вершин
            bool[] visited = new bool[nodes.Count];
            // Вызываем рекурсивную функцию поиска пути
            return DFS(startNodeIndex, endNodeIndex, visited);
        }

        // Рекурсивная функция (Depth-First Search) для поиска пути между двумя вершинами в графе.
        private bool DFS(int currentNodeIndex, int endNodeIndex, bool[] visited)
        {
            // Помечаем текущую вершину как посещенную
            visited[currentNodeIndex] = true;
            // Если текущая вершина равна конечной, значит путь найден
            if (currentNodeIndex == endNodeIndex)
                return true;
            // Проверяем все соседние вершины
            foreach (var edge in edges)
            {
                int neighborNodeIndex = GetNeighborNodeIndex(currentNodeIndex, edge);
                // Если соседняя вершина еще не посещена и есть ребро между текущей и соседней вершинами,
                // вызываем DFS для соседней вершины
                if (!visited[neighborNodeIndex] && IsEdgeBetweenNodes(currentNodeIndex, neighborNodeIndex))
                {
                    if (DFS(neighborNodeIndex, endNodeIndex, visited))
                        return true;
                }
            }
            return false;
        }

        // Метод для нахождения максимальной пропускной способности между вершинами
        private int FindMaxFlow(int sourceNodeIndex, int targetNodeIndex)
        {
            int[,] capacityMatrix = InitializeCapacityMatrix(); // Инициализация матрицы пропускных способностей
            int[] parent = new int[nodes.Count]; // Создание массива для хранения родительских вершин
            int maxFlow = 0; // Инициализация максимального потока
            // Цикл выполняется, пока существует путь от исходной вершины к целевой вершине
            while (BFS(sourceNodeIndex, targetNodeIndex, capacityMatrix, parent))
            {
                int pathFlow = FindPathMaxCapacity(sourceNodeIndex, targetNodeIndex, capacityMatrix, parent); // Нахождение максимальной пропускной способности пути
                int v = targetNodeIndex;
                // Обновление остаточной пропускной способности на ребрах пути
                while (v != sourceNodeIndex)
                {
                    int u = parent[v];
                    capacityMatrix[u, v] -= pathFlow; // Уменьшение пропускной способности на прямом ребре
                    capacityMatrix[v, u] += pathFlow; // Увеличение пропускной способности на обратном ребре
                    v = u; // Обновление текущей вершины v на родительскую вершину u.
                }
                maxFlow += pathFlow; // Увеличение общего потока
            }
            return maxFlow; // Возвращение максимального потока
        }

        // Метод для нахождения максимальной пропускной способности на пути
        private int FindPathMaxCapacity(int sourceNodeIndex, int targetNodeIndex, int[,] capacityMatrix, int[] parent)
        {
            int maxCapacity = int.MaxValue; // Инициализация переменной maxCapacity значением int.MaxValue
            int v = targetNodeIndex; // Инициализация переменной v значением targetNodeIndex
            while (v != sourceNodeIndex) // Цикл выполняется до достижения исходного узла
            {
                int u = parent[v]; // Получение родительского узла v и сохранение его в переменной u
                int capacity = capacityMatrix[u, v]; // Получение пропускной способности между узлами u и v
                if (capacity < maxCapacity) // Проверка, если пропускная способность capacity меньше текущего значения maxCapacity
                    maxCapacity = capacity; // Обновление значения maxCapacity с capacity
                v = u; // Переход к следующему узлу в пути (переход назад)
            }
            v = targetNodeIndex; // Восстановление значения переменной v
            while (v != sourceNodeIndex) // Цикл выполняется до достижения исходного узла
            {
                int u = parent[v]; // Получение родительского узла v и сохранение его в переменной u
                int capacity = capacityMatrix[u, v]; // Получение пропускной способности между узлами u и v
                int weight = capacityMatrix[u, v]; // Получение веса (capacity / weight) ребра между узлами u и v
                if (weight > 0) // Проверка, если вес ребра больше 0
                    maxCapacity = Math.Min(maxCapacity, capacity / weight); // Обновление значения maxCapacity с минимальным значением между текущим maxCapacity и capacity / weight
                v = u; // Переход к следующему узлу в пути (переход назад)
            }
            return maxCapacity; // Возврат найденного значения максимальной пропускной способности на пути
        }

        // Вспомогательный метод для поиска пути в ширину (BFS)
        private bool BFS(int sourceNodeIndex, int targetNodeIndex, int[,] capacityMatrix, int[] parent)
        {
            bool[] visited = new bool[nodes.Count]; // Массив для отслеживания посещенных вершин
            Queue<int> queue = new Queue<int>(); // Очередь для обхода графа в ширину
            queue.Enqueue(sourceNodeIndex); // Добавление исходной вершины в очередь
            visited[sourceNodeIndex] = true; // Пометка исходной вершины как посещенной
            parent[sourceNodeIndex] = -1; // Установка родительской вершины исходной вершины на -1 (неопределено)
            while (queue.Count > 0) // Пока очередь не пуста
            {
                int currentNodeIndex = queue.Dequeue(); // Извлечение вершины из очереди для обработки
                for (int neighborNodeIndex = 0; neighborNodeIndex < nodes.Count; neighborNodeIndex++)
                {
                    // Проверка соседних вершин, которые еще не были посещены и имеют ненулевую пропускную способность
                    if (!visited[neighborNodeIndex] && capacityMatrix[currentNodeIndex, neighborNodeIndex] > 0)
                    {
                        queue.Enqueue(neighborNodeIndex); // Добавление соседней вершины в очередь для дальнейшей обработки
                        visited[neighborNodeIndex] = true; // Пометка соседней вершины как посещенной
                        parent[neighborNodeIndex] = currentNodeIndex; // Запись текущей вершины как родительской для соседней вершины
                    }
                }
            }
            return visited[targetNodeIndex]; // Возвращение значения, указывающего, была ли достигнута целевая вершина при обходе
        }

        // Инициализирует матрицы пропускных способностей ребер в графе.
        private int[,] InitializeCapacityMatrix()
        {
            int[,] capacityMatrix = new int[nodes.Count, nodes.Count];
            for (int i = 0; i < edges.Count; i++)
            {
                var edge = edges[i];
                int startIndex = GetNodeIndex(edge.Item1);
                int endIndex = GetNodeIndex(edge.Item2);
                if (i < edgeWeights.Count)
                {
                    // Получаем вес ребра из коллекции edgeWeights по соответствующему индексу
                    string weightString = edgeWeights[i];
                    // Преобразуем вес ребра из строки в целое число
                    if (int.TryParse(weightString, out int weight))
                    {
                        // Устанавливаем пропускную способность ребра равной весу
                        capacityMatrix[startIndex, endIndex] = weight;
                    }
                    else
                    {
                        // Обработка ошибки при некорректном весе ребра
                        MessageBox.Show("Введено некорректное значение веса ребра. Пожалуйста, введите только числа.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // Установка значения по умолчанию (например, 0) или выбор альтернативного подхода
                        capacityMatrix[startIndex, endIndex] = 0;
                    }
                }
                else
                {
                    // Вес ребра не определен, установка значения по умолчанию
                    capacityMatrix[startIndex, endIndex] = 0;
                }
            }
            return capacityMatrix;
        }

        // Выполняет поиск индекса узла в коллекции nodes по заданной точке point.
        private int GetNodeIndex(Point point)
        {
            // Проходим по коллекции узлов
            for (int i = 0; i < nodes.Count; i++)
            {
                // Проверяем, совпадает ли позиция текущего узла с заданной точкой
                if (nodes[i].Point == point)
                    return i;  // Если найдено совпадение, возвращаем индекс узла
            }
            return -1;  // Если совпадение не найдено, возвращаем -1
        }

        // Данный метод используется для получения индекса соседней вершины (узла) на основе текущей вершины (узла) и ребра, которое их соединяет.
        private int GetNeighborNodeIndex(int currentNodeIndex, Tuple<Point, Point> edge)
        {
            // Проверяем, совпадает ли первая точка ребра с позицией текущего узла
            if (edge.Item1 == nodes[currentNodeIndex].Point)
                return GetNodeIndex(edge.Item2);  // Если есть совпадение, возвращаем индекс второй точки (соседнего узла)
            else
                return GetNodeIndex(edge.Item1);  // Если нет совпадения, предполагаем, что вторая точка является соседним узлом и возвращаем его индекс
        }

        // Проверяет, существует ли ребро между двумя узлами, заданными их индексами nodeIndex1 и nodeIndex2. 
        private bool IsEdgeBetweenNodes(int nodeIndex1, int nodeIndex2)
        {
            // Получаем позиции узлов по их индексам
            Point point1 = nodes[nodeIndex1].Point;
            Point point2 = nodes[nodeIndex2].Point;
            // Проходим по коллекции ребер
            foreach (var edge in edges)
            {
                // Проверяем, существует ли ребро между двумя узлами
                if ((edge.Item1 == point1 && edge.Item2 == point2) ||
                    (edge.Item1 == point2 && edge.Item2 == point1))
                    return true;  // Если ребро найдено, возвращаем true
            }
            return false;  // Если ребро не найдено, возвращаем false
        }

        // Метод для удаления вершины и связанных с ней ребер
        private void RemoveNodeAndEdges(int nodeIndex)
        {
            if (nodeIndex < 0 || nodeIndex >= nodes.Count)
                return;
            // Удаляем ребра, связанные с удаляемой вершиной
            for (int i = edges.Count - 1; i >= 0; i--)
            {
                Tuple<Point, Point> edge = edges[i];
                if (edge.Item1 == nodes[nodeIndex].Point || edge.Item2 == nodes[nodeIndex].Point)
                {
                    edges.RemoveAt(i);
                }
            }
            // Удаляем вершину
            nodes.RemoveAt(nodeIndex);
            // Сбрасываем выбранный элемент
            selectedNodeIndex = -1;
            selectedEdgeIndex = -1;
            // Перерисовываем компонент
            Invalidate();
        }

        // Метод для удаления ребра
        private void RemoveEdge(int edgeIndex)
        {
            // Проверяем, существует ли ребро с указанным индексом
            if (edgeIndex >= 0 && edgeIndex < edges.Count)
            {
                // Удаляем ребро из списка ребер
                edges.RemoveAt(edgeIndex);
                // Проверяем, существует ли сохраненный вес для данного ребра
                if (edgeIndex < edgeWeights.Count)
                {
                    // Удаляем вес ребра из списка весов всех ребер
                    edgeWeights.RemoveAt(edgeIndex);
                }
                // Перерисовываем элемент управления
                Invalidate();
            }
        }

        // Метод для проверки, попадает ли точка на ребро
        private bool IsPointOnEdge(Point point, Point start, Point end)
        {
            const int tolerance = 3; // Допустимое отклонение от ребра
            float distance = DistancePointToLine(point, start, end);
            return distance <= tolerance;
        }

        // Метод для вычисления расстояния от точки до линии
        private float DistancePointToLine(Point point, Point start, Point end)
        {
            // Вычисляем разности координат
            float a = point.X - start.X;
            float b = point.Y - start.Y;
            float c = end.X - start.X;
            float d = end.Y - start.Y;
            // Вычисляем скалярное произведение и квадрат длины отрезка
            float dot = a * c + b * d;
            float lenSq = c * c + d * d;
            // Вычисляем параметр
            float param = dot / lenSq;
            float xx, yy;
            // Если параметр меньше 0, точка ближе к началу отрезка
            if (param < 0)
            {
                xx = start.X;
                yy = start.Y;
            }
            // Если параметр больше 1, точка ближе к концу отрезка
            else if (param > 1)
            {
                xx = end.X;
                yy = end.Y;
            }
            // Иначе точка лежит на отрезке
            else
            {
                xx = start.X + param * c;
                yy = start.Y + param * d;
            }
            // Вычисляем разности координат точки и ближайшей точки на линии
            float dx = point.X - xx;
            float dy = point.Y - yy;
            // Вычисляем и возвращаем расстояние между точкой и ближайшей точкой на линии
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        // Добавляет новый узел в граф с указанной позицией (Point).
        public void AddNode(Point point)
        {
            int nodeIndex = nodes.Count + 1; // Порядковый номер вершины
            Node newNode = new Node(point, nodeIndex);
            OnNodeAdd();
            nodes.Add(newNode);
            Invalidate(); // Перерисовка
        }

        // Добавляет новое ребро в граф между двумя узлами с указанными начальной (start) и конечной (end) позициями (Point).
        public void AddEdge(Point start, Point end)
        {
            edges.Add(Tuple.Create(start, end));
            Invalidate(); // Перерисовка
        }

        // Состояния объекта
        public enum ObjStates
        {
            osConvex,
            osConcavity
        }
        public ObjStates _ObjState;

        // Предоставляет доступ к состоянию объекта (ObjStates).
        public virtual ObjStates ObjState
        {
            get
            {
                return _ObjState;
            }
            set
            {
                if (value != _ObjState)
                {
                    _ObjState = value;
                    Invalidate();
                }
            }
        }

        // Представляет свойство DarkColor, которое предоставляет доступ к цвету Color объекта.
        public Color DarkColor
        {
            get
            {
                return _DarkColor;
            }
            set
            {
                if (value != _DarkColor)
                {
                    _DarkColor = value;
                    Invalidate();
                }
            }
        }

        // Представляет свойство LightColor, которое предоставляет доступ к цвету Color объекта.
        public Color LightColor
        {
            get
            {
                return _LightColor;
            }
            set
            {
                if (value != _LightColor)
                {
                    _LightColor = value;
                    Invalidate();
                }
            }
        }

        // Этот метод используется для изменения состояния объекта.
        public virtual void State()
        {
            int I = (int)_ObjState + 1;
            I = I > 1 ? 0 : I;
            ObjState = (ObjStates)I;
            OnChangeState();
        }

        // Определяет логику отрисовки компонента или элемента управления.
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen LightPen = new Pen(_LightColor);
            Pen DarkPen = new Pen(_DarkColor);
            // Отрисовка рамки
            for (int i = 0; i < Height / 25; i++)
            {
                Point[] D =
                    {
                        new Point(Width-i,i),
                        new Point(i,i),
                        new Point(i,Height-i)
                        };
                Point[] L =
                    {
                        new Point(Width-i,i),
                        new Point(Width-i,Height-i),
                        new Point(i,Height-i)
                        };
                if (ObjState == 0)
                {
                    e.Graphics.DrawLines(DarkPen, D);
                    e.Graphics.DrawLines(LightPen, L);
                }
                else
                {
                    e.Graphics.DrawLines(DarkPen, L);
                    e.Graphics.DrawLines(LightPen, D);
                }
            }
            // Отрисовка ребер
            for (int i = 0; i < edges.Count; i++)
            {
                Tuple<Point, Point> edge = edges[i];
                Point startPoint = edge.Item1;
                Point endPoint = edge.Item2;
                // Рисуем ребро
                g.DrawLine(DarkPen, startPoint, endPoint);
                // Рассчитываем координаты середины ребра
                int midX = (startPoint.X + endPoint.X) / 2;
                int midY = (startPoint.Y + endPoint.Y) / 2;
                // Получаем вес ребра из коллекции или массива
                string weight = GetEdgeWeight(i);
                // Рисуем надпись с весом ребра
                g.DrawString(weight, Font, Brushes.Black, midX, midY);
                // Рисуем стрелку на конце ребра
                DrawArrow(g, startPoint, endPoint, DarkPen);
            }
            // Отрисовка вершин
            for (int i = 0; i < nodes.Count; i++)
            {
                Rectangle rect = new Rectangle(nodes[i].Point.X - 15, nodes[i].Point.Y - 15, nodeSize, nodeSize);
                if (i == selectedNodeIndex)
                {
                    g.FillEllipse(Brushes.LightBlue, rect); // Заполняем выбранную вершину другим цветом
                }
                else
                {
                    g.FillEllipse(Brushes.White, rect);
                }
                g.DrawEllipse(Pens.Black, rect);
                string nodeText = (i + 1).ToString();
                Font nodeFont = new Font(Font.FontFamily, 12); // Установка размера шрифта
                SizeF textSize = g.MeasureString(nodeText, nodeFont);
                PointF textPosition = new PointF(nodes[i].Point.X - textSize.Width / 2, nodes[i].Point.Y - textSize.Height / 2);
                g.DrawString(nodeText, nodeFont, Brushes.Black, textPosition);
            }
        }

        private void DrawArrow(Graphics g, Point startPoint, Point endPoint, Pen pen)
        {
            int arrowSize = 25; // Размер стрелки
            int arrowAngle = 30; // Угол между линиями стрелки (в градусах)
            // Вычисляем середину ребра
            int midX = (startPoint.X + endPoint.X) / 2;
            int midY = (startPoint.Y + endPoint.Y) / 2;
            // Вычисляем направление вектора ребра
            double angle = Math.Atan2(startPoint.Y - endPoint.Y, startPoint.X - endPoint.X);
            // Вычисляем координаты конца стрелки
            int arrowEndX = midX - (int)(Math.Cos(angle) * arrowSize);
            int arrowEndY = midY - (int)(Math.Sin(angle) * arrowSize);
            // Рисуем линию стрелки
            g.DrawLine(pen, startPoint, new Point(arrowEndX, arrowEndY));
            // Рисуем первую часть стрелки
            int arrowPart1X = arrowEndX + (int)(Math.Cos(angle + Math.PI * arrowAngle / 180) * arrowSize);
            int arrowPart1Y = arrowEndY + (int)(Math.Sin(angle + Math.PI * arrowAngle / 180) * arrowSize);
            g.DrawLine(pen, new Point(arrowEndX, arrowEndY), new Point(arrowPart1X, arrowPart1Y));
            // Рисуем вторую часть стрелки
            int arrowPart2X = arrowEndX + (int)(Math.Cos(angle - Math.PI * arrowAngle / 180) * arrowSize);
            int arrowPart2Y = arrowEndY + (int)(Math.Sin(angle - Math.PI * arrowAngle / 180) * arrowSize);
            g.DrawLine(pen, new Point(arrowEndX, arrowEndY), new Point(arrowPart2X, arrowPart2Y));
        }

        // Метод, который возвращает строковое представление веса ребра по заданному индексу edgeIndex. 
        private string GetEdgeWeight(int edgeIndex)
        {
            // Проверяем, есть ли уже сохраненный вес для данного ребра
            if (edgeIndex < edgeWeights.Count)
            {
                // Возвращаем сохраненный вес
                return edgeWeights[edgeIndex];
            }
            else
            {
                // Предлагаем пользователю ввести вес ребра
                string weightString = Microsoft.VisualBasic.Interaction.InputBox("Введите вес ребра " + edgeIndex.ToString(), "Ввод веса ребра");
                // Проверяем, было ли введено значение
                if (!string.IsNullOrEmpty(weightString))
                {
                    // Проверяем, содержит ли строка только числа
                    int weight;
                    if (int.TryParse(weightString, out weight))
                    {
                        // Сохраняем введенный вес в коллекцию или массив
                        edgeWeights.Add(weightString);
                        OnEdgeAdd();
                        // Возвращаем введенный вес
                        return weightString;
                    }
                    else
                    {
                        // Если введено некорректное значение, выводим сообщение об ошибке
                        DialogResult result = MessageBox.Show("Введено некорректное значение веса. Желаете отменить рисование ребра?", "Ошибка ввода", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (result == DialogResult.Yes)
                        {
                            // Отменяем рисование ребра
                            RemoveEdge(edgeIndex);
                        }
                        // Возвращаем null для указания отсутствия веса
                        return null;
                    }
                }
                else
                {
                    // Если значение не было введено, проверяем, было ли окно ввода активировано
                    if (Microsoft.VisualBasic.Interaction.InputBox("", "", "") == "")
                    {
                        // Если окно ввода было закрыто без ввода значения, предлагаем отменить рисование ребра
                        DialogResult result = MessageBox.Show("Введение веса ребра было отменено. Желаете отменить рисование ребра?", "Отмена", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            // Отменяем рисование ребра
                            RemoveEdge(edgeIndex);
                        }
                        // Возвращаем null для указания отсутствия веса
                        return null;
                    }
                    else
                    {
                        // Если окно ввода было активировано повторно, рекурсивно вызываем метод для повторного ввода значения
                        return GetEdgeWeight(edgeIndex);
                    }
                }
            }
        }

        // Определяет вложенный приватный класс Node, который представляет узел графа.
        private class Node
        {
            public Point Point { get; set; }
            public int Index { get; private set; }
            public Color Color { get; internal set; }
            public Node(Point point, int index)
            {
                Point = point;
                Index = index;
            }
        }

        // События класса
        protected event EventHandler _OnChangeState;
        protected event EventHandler _OnNodeAdd;
        protected event EventHandler _OnRemoveNode;
        protected event EventHandler _OnEdgeAdd;
        protected event EventHandler _OnNodeChoose;
        protected event EventHandler _OnRemoveEdge;
        protected event EventHandler _OnRemoveAll;
        protected event EventHandler _OnFindMaxFlow;

        public event EventHandler EdgeAdd
        {
            add
            {
                _OnEdgeAdd += value;
            }
            remove
            {
                _OnEdgeAdd -= value;
            }
        }
        public event EventHandler NodeAdd
        {
            add
            {
                _OnNodeAdd += value;
            }
            remove
            {
                _OnNodeAdd -= value;
            }
        }
        public event EventHandler NodeChoose
        {
            add
            {
                _OnNodeChoose += value;
            }
            remove
            {
                _OnNodeChoose -= value;
            }
        }
        public event EventHandler RemoveNode
        {
            add
            {
                _OnRemoveNode += value;
            }
            remove
            {
                _OnRemoveNode -= value;
            }
        }
        public event EventHandler RemoveEdge1
        {
            add
            {
                _OnRemoveEdge += value;
            }
            remove
            {
                _OnRemoveEdge -= value;
            }
        }
        public event EventHandler RemoveAll
        {
            add
            {
                _OnRemoveAll += value;
            }
            remove
            {
                _OnRemoveAll -= value;
            }
        }
        public event EventHandler ChangeState
        {
            add
            {
                _OnChangeState += value;
            }
            remove
            {
                _OnChangeState -= value;
            }
        }
        public event EventHandler FindMaxFloww
        {
            add
            {
                _OnFindMaxFlow += value;
            }
            remove
            {
                _OnFindMaxFlow -= value;
            }
        }
        // Вызывает событие, которое уведомляет о действии.
        protected void OnEdgeAdd()
        {
            if (_OnEdgeAdd != null)
            {
                _OnEdgeAdd.Invoke(this, new EventArgs());
            }
        }
        protected void OnNodeAdd()
        {
            if (_OnNodeAdd != null)
            {
                _OnNodeAdd.Invoke(this, new EventArgs());
            }
        }
        protected void OnNodeChoose()
        {
            if (_OnNodeChoose != null)
            {
                _OnNodeChoose.Invoke(this, new EventArgs());
            }
        }
        protected void OnRemoveNode()
        {
            if (_OnRemoveNode != null)
            {
                _OnRemoveNode.Invoke(this, new EventArgs());
            }
        }
        protected void OnRemoveEdge()
        {
            if (_OnRemoveEdge != null)
            {
                _OnRemoveEdge.Invoke(this, new EventArgs());
            }
        }
        protected void OnRemoveAll()
        {
            if (_OnRemoveAll != null)
            {
                _OnRemoveAll.Invoke(this, new EventArgs());
            }
        }
        protected void OnChangeState()
        {
            if (_OnChangeState != null)
            {
                _OnChangeState.Invoke(this, new EventArgs());
            }
        }
        protected void OnFindMaxFlow()
        {
            if (_OnFindMaxFlow != null)
            {
                _OnFindMaxFlow.Invoke(this, new EventArgs());
            }
        }
    }
}