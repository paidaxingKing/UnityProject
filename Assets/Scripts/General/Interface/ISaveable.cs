using UnityEngine;

public interface ISaveable
{
    public void LoadData(GameData data);

    public void SaveData(ref GameData data);
    //ref表示传递的是参数引用的具体地址，而不是引用本身,即在函数内部可以为data另赋值
    /*
     * 即在函数内可以有data = new GameData()，原来地址存放的实例就被更改为了新的实例,即可以修改地址本身存放的内容
     * ref 不是为了解决“能不能改属性”的技术限制，而是为了“架构的安全性、未来的可扩展性、以及代码语义的严谨性”。
     * 如果出现问题可以直接在**方法内部**执行data = new GameData()从而刷新游戏数据，外部参数也会跟着改变
     */
}
