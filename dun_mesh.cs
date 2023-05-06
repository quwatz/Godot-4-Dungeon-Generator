using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Threading.Tasks;

[Tool]
public partial class dun_mesh : Node3D
{
	[Export]
	NodePath gridMapPath;
    GridMap gridMap;
    [ Export]
	private bool start { get { return start; } set { CreateDungeon(); } }

	PackedScene dunCellScene = (PackedScene)ResourceLoader.Load("res://imports/DunCell.tscn");

	Dictionary<String , Vector3I> directions = new Dictionary<String , Vector3I>() {
		{"up",Vector3I.Forward},
		{"down",Vector3I.Back},
		{"left",Vector3I.Left},
		{"right",Vector3I.Right}};


	private void handleNone(Node3D cell,String dir)
	{
		cell.Call( "remove_door_" + dir );
	}
    private void handle00( Node3D cell , String dir )
    {
        cell.Call( "remove_door_" + dir );
        cell.Call( "remove_wall_" + dir );
    }
    private void handle01( Node3D cell , String dir )
    {
        cell.Call( "remove_door_" + dir );
    }
    private void handle02( Node3D cell , String dir )
    {
        cell.Call( "remove_door_" + dir );
        cell.Call( "remove_wall_" + dir );
    }
    private void handle10( Node3D cell , String dir )
    {
        cell.Call( "remove_door_" + dir );
    }
    private void handle11( Node3D cell , String dir )
    {
        cell.Call( "remove_door_" + dir );
        cell.Call( "remove_wall_" + dir );
    }
    private void handle12( Node3D cell , String dir )
    {
        cell.Call( "remove_door_" + dir );
        cell.Call( "remove_wall_" + dir );
    }
    private void handle20( Node3D cell , String dir )
    {
        cell.Call( "remove_door_" + dir );
        cell.Call( "remove_wall_" + dir );
    }
    private void handle21( Node3D cell , String dir )
    {
        cell.Call( "remove_wall_" + dir );
    }
    private void handle22( Node3D cell , String dir )
    {
        cell.Call( "remove_door_" + dir );
        cell.Call( "remove_wall_" + dir );
    }

    public override void _Ready()
	{
		gridMap = GetNode<GridMap>(gridMapPath);
		
	}

	private async void CreateDungeon()
	{
        foreach(Node c in GetChildren())
        {
            RemoveChild( c );
            c.QueueFree();
        }

        int t = 0;

        foreach (Vector3I cell in gridMap.GetUsedCells())
        {
            int cellIndex = gridMap.GetCellItem(cell);
            if ( cellIndex <= 2 && cellIndex >= 0 )
            {
                Node3D dunCell = (Node3D)dunCellScene.Instantiate();
                dunCell.Position = ( Vector3 )cell + new Vector3(0.5f,0,0.5f);
                AddChild( dunCell );
                t++;
                for (int i=0 ;i<4 ;i++)
                {
                    Vector3I cellN = cell + directions.Values.ToList()[i];
                    int cellNIndex = gridMap.GetCellItem( cellN );
                    if (cellNIndex == -1 || cellNIndex ==3 )
                    {
                        handleNone( dunCell , directions.Keys.ToList()[i]);
                    }
                    else
                    {
                        String key = cell.ToString() + cellN.ToString() ;
                        Call( "handle" + key , dunCell , directions.Keys.ToList()[i] );
                    }
                }
                if ( t % 10 == 9 ) await Task.Delay(TimeSpan.FromSeconds(0));
            }
        }

	}
}

