namespace ToolkitEngine.Shooter
{
	public interface IAmmoTypeLinkable
    {
        BaseAmmoCache ammoCache { get; set; }
        AmmoType ammoType { get; }
    }
}