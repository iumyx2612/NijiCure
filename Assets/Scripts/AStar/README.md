# A* Algorithm
## Pseudo code
`OPEN` <- set of nodes to be evaluated  
`CLOSED` <- set of nodes alr evaluated  
add the start node to `OPEN`

loop:  
    &nbsp;&nbsp;&nbsp;&nbsp;`current` = node in `OPEN` with lowest `F_cost`  
    &nbsp;&nbsp;&nbsp;&nbsp;remove `current` from `OPEN`  
    &nbsp;&nbsp;&nbsp;&nbsp;add `current` to `CLOSED`    
    &nbsp;&nbsp;&nbsp;&nbsp;if `current` is the target node:   
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; return  
    &nbsp;&nbsp;&nbsp;&nbsp;foreach `neighbor` of `current` node  
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;if `neighbor` is not traversable or `neighbor` in `CLOSED`:  
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;skip to next `neighbor`  
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;if new path to `neighbor` is shorter OR `neighbor` is not in `OPEN`  
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;set F_cost of `neighbor`  
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;set parent of `neighbor` to `current`  
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;if `neighbor` not in `OPEN`:  
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;add `neighbor` to `OPEN`
 
## Code
[Node](Node.cs): Represents the node  
[Heap](Heap.cs): Heap data structure to optimize A*  
[Grid](Grid.cs): Spawn the entire grid to run the A*  