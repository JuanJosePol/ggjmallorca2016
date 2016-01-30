using UnityEngine;

[System.Serializable]
public class Game
{
    private string _name;
    private float _requieredEffort;
    private float _progress;

    public string name
    {
        get { return _name; }
        set { _name = value; }
    }
    public float requieredEffort
    {
        get { return _requieredEffort; }
        set { _requieredEffort = value; }
    }
    public float progress
    {
        get { return _progress; }
        set { _progress = value; }
    }

    public Game(string gameName, float effort)
    {
        name = gameName;
        progress = 0;
	    this.requieredEffort = effort;
	    InfoSlotListManager.instanceGameList.AddGameSlot(this);
    }
    
    public void Develop(int jammersWorking)
    {
        progress += jammersWorking * Time.deltaTime / requieredEffort;
    }
}
