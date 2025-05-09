using System.Collections.Generic;

public class ExistManager
{
    Dictionary<(int,int), int> map = new Dictionary<(int,int), int>();

    private static ExistManager _instance = new ExistManager();
    public static ExistManager GetInstance(){
        return _instance;
    }

    public bool ContainsKey(int wx, int wy)
    {
        int x = ((Const.fieldLengthX / 2) + wx) % Const.fieldLengthX - (Const.fieldLengthX / 2);
        int y = ((Const.fieldLengthY / 2) + wy) % Const.fieldLengthY - (Const.fieldLengthY / 2);

        return map.ContainsKey((x, y));
    }

    public void Put(int wx, int wy)
    {
        int x = ((Const.fieldLengthX / 2) + wx) % Const.fieldLengthX - (Const.fieldLengthX / 2);
        int y = ((Const.fieldLengthY / 2) + wy) % Const.fieldLengthY - (Const.fieldLengthY / 2);

        if (ContainsKey(x, y)) return;

        map[(wx, wy)] = 1;
    }

    public void Remove(int wx, int wy)
    {
        int x = ((Const.fieldLengthX / 2) + wx) % Const.fieldLengthX - (Const.fieldLengthX / 2);
        int y = ((Const.fieldLengthY / 2) + wy) % Const.fieldLengthY - (Const.fieldLengthY / 2);

        if (!ContainsKey(x, y)) return;

        map.Remove((wx, wy));
    }
}
