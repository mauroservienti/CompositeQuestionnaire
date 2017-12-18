namespace ITOps.ViewModelComposition
{
    public interface ISubscribeToCompositionEvents : IInterceptRoutes
    {
        void Subscribe(IPublishCompositionEvents publisher);
    }
}
