namespace AdasIt.Andor.Infrastructure;

public interface IFeatureFlag
{
    public bool IsEnabled(string featureName);
}
