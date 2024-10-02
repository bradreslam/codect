namespace BLL.Interfaces;

public interface IFeature
{
	void Deactivate();
	double Activate(double power);
}