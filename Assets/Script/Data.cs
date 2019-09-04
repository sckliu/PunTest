[System.Serializable]
public struct Data
{
    public float speed;
    public float damage;
    public string name;
    public float hp;
    public float hitArea;
    public float dia;

    public Data(float speed, float damage, string name, float hp, float hitArea, float dia)
    {
        this.speed = speed;
        this.damage = damage;
        this.name = name;
        this.hp = hp;
        this.hitArea = hitArea;
        //[Header("擊退距離")]
        this.dia = dia;

    }


}

