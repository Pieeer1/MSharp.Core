using Microsoft.Extensions.DependencyInjection;
using MSharp.Core.GraphQL.Attributes;
using MSharp.Core.GraphQL.Extensions;

namespace MSharp.GraphQL.Tests;
public class GraphQLExtensionTests
{

    [Fact]
    public void Test_AutomaticGraphQLInjections()
    { 
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddGraphQLServer().AddAutomaticGraphQLInjections().InitializeOnStartup();
        //essentially a runtime check. should check if the types are added to the schema at a later time.
    }

    [Query]
    [ExtendObjectType("Query")]
    private class TestQuery()
    { 
        
    }
    [Mutation]
    [ExtendObjectType("Mutation")]
    private class TestMutation()
    {
        
    }
    [Subscription]
    [ExtendObjectType("Subscription")]
    private class TestSubscription()
    {
        
    }

}
