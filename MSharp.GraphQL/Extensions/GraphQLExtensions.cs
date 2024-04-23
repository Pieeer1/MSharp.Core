using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MSharp.Core.GraphQL.Attributes;
using System.Diagnostics;
using System.Reflection;

namespace MSharp.Core.GraphQL.Extensions;
public static class GraphQLExtensions
{
    /// <summary>
    /// Automatic Handling of Query and Mutation Types within Hot Chocolate. Usage:
    /// When creating a new Mutation, add the appropriate attributes to the class:
    /// [Mutation][ExtendObjectType("Mutation")]
    /// When creating a new Query, add the appropriate attributes to the class:
    /// [Query][ExtendObjectType("Query")]
    /// When creating a new Subscription, add the appropriate attributes to the class:
    /// [Subscription][ExtendObjectType("Subscription")]
    /// </summary>
    /// <param name="requestExecutorBuilder">The Hot Chocolate Request Executor</param>
    public static IRequestExecutorBuilder AddAutomaticGraphQLInjections(this IRequestExecutorBuilder requestExecutorBuilder)
    {
        StackFrame stackFrame = new StackFrame(1);

        Type aspNetAssemblyType = stackFrame.GetMethod()?.DeclaringType ?? throw new NullReferenceException("Could not accurately determine Stack type, Are you injecting this in the Program?");

        var queries = Assembly.GetAssembly(aspNetAssemblyType)?.GetTypes().Where(x => x.GetCustomAttribute<QueryAttribute>() is not null) ?? Enumerable.Empty<Type>();
        var mutations = Assembly.GetAssembly(aspNetAssemblyType)?.GetTypes().Where(x => x.GetCustomAttribute<MutationAttribute>() is not null) ?? Enumerable.Empty<Type>();
        var subscriptions = Assembly.GetAssembly(aspNetAssemblyType)?.GetTypes().Where(x => x.GetCustomAttribute<SubscriptionAttribute>() is not null) ?? Enumerable.Empty<Type>();
        if (queries.Any())
        {
            requestExecutorBuilder.AddQueryType(x => x.Name("Query"));
        }
        if (mutations.Any())
        {
            requestExecutorBuilder.AddMutationType(x => x.Name("Mutation"));
        }
        if (subscriptions.Any())
        {
            requestExecutorBuilder.AddSubscriptionType(x => x.Name("Subscription"));
        }

        foreach (var query in queries)
        {
            requestExecutorBuilder.AddType(query);
        }
        foreach (var query in mutations)
        {
            requestExecutorBuilder.AddType(query);
        }
        foreach (var query in subscriptions)
        {
            requestExecutorBuilder.AddType(query);
        }
        return requestExecutorBuilder;
    }
}
