[System.Serializable]
public struct Data
{
    public float speed;
    public float damage;
    public string name;
    public float hp;
    public float hitArea;

    public Data(float speed, float damage, string name, float hp, float hitArea)
    {
        this.speed = speed;
        this.damage = damage;
        this.name = name;
        this.hp = hp;
        this.hitArea = hitArea;

    }


}

