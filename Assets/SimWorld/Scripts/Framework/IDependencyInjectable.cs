namespace SimWorld
{
	/// <summary>
	/// TODO: I don't think I will finally use this...
	/// </summary>
	public interface IDependencyInjectable
	{
		public void InitializeManager();
	}
	public interface IDependencyInjectable<in T>
	{
		public void InitializeManager(T gameLauncherManager);
	}

	public interface IDependencyInjectable<in T, in T2>
	{
		public void InitializeManager(T t, T2 t2);
	}

	public interface IDependencyInjectable<in T, in T2, in T3>
	{
		public void InitializeManager(T t, T2 t2, T3 t3);
	}

	public interface IDependencyInjectable<in T, in T2, in T3, in T4>
	{
		public void InitializeManager(T t, T2 t2, T3 t3, T4 t4);
	}
}
