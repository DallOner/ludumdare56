public interface IWeapon
{
    void Shoot();
    void Reload();
    bool CanShoot { get; }
    bool CanReload { get; }
}
