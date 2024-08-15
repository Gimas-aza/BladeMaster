namespace Assets.MVP.Model
{
    public interface IModel
    {
        void LoadLevel(int sceneIndex);
        void LoadNextLevel();
        void LoadPreviousLevel();
    }
}
