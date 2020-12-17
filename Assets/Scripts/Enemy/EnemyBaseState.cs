// 抽象类，在抽象类型中可以定义函数方法
// 但是不必实现这些函数的功能，只需要写函数的声明
// 然后用子类来实现这些方法
public abstract class EnemyBaseState
{
    public abstract void EnterState(Enemy enemy);

    // 保持当前状态
    public abstract void OnUpdate(Enemy enemy);
}
