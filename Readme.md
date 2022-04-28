# Classes

## Procura Construtiva

### Data Structure

- `expansoes` and `geracoes` - statistical data
- `cost` - used for UCS algorithm
- `results`, `visitados` - marks the results and the visited nodes
- `queue`, `stack` and `priorityQueue` - used to each specific algorithm

### **Largura Primeiro**

Largura Primeiro or Breadth First Search (BFS) algorithm, works as follows:

**Starts with an empty queue and adds the root node**

- removes node from queue (FIFO)
- verifies if node has been visited, if true, then dequeue the next node
- verifies if node contains the solution, if true, then finish the operation and return the current node's result.
- each node will *spawn* it's children and enqueue them to the end of the queue

**repeat this process until the queue is empty or a solution has been found**


### **Profundidade Primeiro**

Profundidade Primeiro or Depth First Search (DFS) algorithm, works as follows:

**Starts with an empty stack and adds the root node**

- removes last node in the stack (LIFO)
- verifies if node has been visited, if true, then dequeue the next node
- verifies if node contains the solution, if true, then finish the operation and return the current node's result.
- each node will *spawn* its children and add them to the end of the stack
- recursively call the dfs function with the new stack so it start with the children of the last node until there are no more children

**repeat this process until the stack is empty or a solution has been found**

### **Custo Uniforme**

Custo Uniform or Uniform Cost Search (UCS) algorithm, works as follows:

**Starts with an empty _priority_ queue and adds the root node**

- removes node from queue (FIFO)
- verifies if node has been visited, if true, then dequeue the next node
- verifies if node contains the solution, if true, then finish the operation and return the current node's result.
- each node will *spawn* it's children and enqueue them to the queue
- the ***priority queue*** will then sort the nodes by cost 

**repeat this process until the queue is empty or a solution has been found**

*note*: the cost value can be anything, i.e number of steps, interactions, etc. For the NoThreeLine problem, we give a cost based on the number of checkers in the board. 