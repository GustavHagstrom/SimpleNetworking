//using Autofac;

//namespace SimpleNetworking.DI
//{
//    internal class ServiceLocator : IServiceLocator
//    {
//        private static IServiceLocator serviceLocator;
//        private IContainer container;
//        private ServiceLocator()
//        {
//            container = ConfigureContainer();
//            container.BeginLifetimeScope();
//        }
//        private IContainer ConfigureContainer()
//        {
//            var builder = new ContainerBuilder();
//            //builder.Register<ILogger>(l =>
//            //{
//            //    return new LoggerConfiguration()
//            //    .WriteTo.Console()
//            //    .WriteTo.Debug()
//            //    .CreateLogger();
//            //}).SingleInstance();
//            builder.RegisterType<ServerClientTcpHandler>().As<IServerClientTcpHandler>();
//            builder.RegisterType<UserClientTcpHandler>().As<IUserClientTcpHandler>();
//            builder.RegisterType<UserClient>().As<IUserClient>();
//            builder.RegisterType<ServerClient>().As<IServerClient>();
//            builder.RegisterType<Server>().As<IServer>();
//            builder.RegisterType<PacketHandlerBuilder>().As<IPacketHandlerBuilder>();
//            builder.RegisterType<PacketHandler>().As<IPacketHandler>();
//            builder.RegisterType<Packet>().As<IPacket>();

//            return builder.Build();
//        }
//        public static IServiceLocator Instance
//        {
//            get
//            {
//                if (serviceLocator == null)
//                {
//                    return new ServiceLocator();
//                }
//                return serviceLocator;
//            }
//        }
//        public T Get<T>()
//        {
//            return container.Resolve<T>();
//        }
//    }
//}
