// <auto-generated>

namespace Swagger.Admin
{
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for AdminAdventureAPI.
    /// </summary>
    public static partial class AdminAdventureAPIExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='searchQuery'>
            /// Search for values within the product name, for instance &lt;b&gt;bike
            /// stand&lt;/b&gt; or &lt;b&gt;mountain&lt;/b&gt;
            /// </param>
            /// <param name='color'>
            /// One or more colors to filter on, for instance &lt;b&gt;black&lt;/b&gt; or
            /// &lt;b&gt;black, blue&lt;/b&gt;
            /// </param>
            /// <param name='orderBy'>
            /// By default the results are ordered by &lt;b&gt;name (ascending)&lt;/b&gt;
            /// You can order by &lt;b&gt;color, color desc, color asc, name, name asc,
            /// name desc&lt;/b&gt; or a combination.
            /// </param>
            /// <param name='fields'>
            /// The fields you want to see in the results. For instance &lt;b&gt;id, name,
            /// color&lt;/b&gt;
            /// </param>
            /// <param name='pageNumber'>
            /// Default = 1.
            /// </param>
            /// <param name='pageSize'>
            /// Default = 10. Maximum = 20.
            /// </param>
            public static IList<Product> ApiAdminProductsGet(this IAdminAdventureAPI operations, string searchQuery = default(string), string color = default(string), string orderBy = default(string), string fields = default(string), int? pageNumber = default(int?), int? pageSize = default(int?))
            {
                return operations.ApiAdminProductsGetAsync(searchQuery, color, orderBy, fields, pageNumber, pageSize).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='searchQuery'>
            /// Search for values within the product name, for instance &lt;b&gt;bike
            /// stand&lt;/b&gt; or &lt;b&gt;mountain&lt;/b&gt;
            /// </param>
            /// <param name='color'>
            /// One or more colors to filter on, for instance &lt;b&gt;black&lt;/b&gt; or
            /// &lt;b&gt;black, blue&lt;/b&gt;
            /// </param>
            /// <param name='orderBy'>
            /// By default the results are ordered by &lt;b&gt;name (ascending)&lt;/b&gt;
            /// You can order by &lt;b&gt;color, color desc, color asc, name, name asc,
            /// name desc&lt;/b&gt; or a combination.
            /// </param>
            /// <param name='fields'>
            /// The fields you want to see in the results. For instance &lt;b&gt;id, name,
            /// color&lt;/b&gt;
            /// </param>
            /// <param name='pageNumber'>
            /// Default = 1.
            /// </param>
            /// <param name='pageSize'>
            /// Default = 10. Maximum = 20.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<Product>> ApiAdminProductsGetAsync(this IAdminAdventureAPI operations, string searchQuery = default(string), string color = default(string), string orderBy = default(string), string fields = default(string), int? pageNumber = default(int?), int? pageSize = default(int?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiAdminProductsGetWithHttpMessagesAsync(searchQuery, color, orderBy, fields, pageNumber, pageSize, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='product'>
            /// </param>
            public static object ApiAdminProductsPost(this IAdminAdventureAPI operations, CreateProduct product = default(CreateProduct))
            {
                return operations.ApiAdminProductsPostAsync(product).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='product'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ApiAdminProductsPostAsync(this IAdminAdventureAPI operations, CreateProduct product = default(CreateProduct), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiAdminProductsPostWithHttpMessagesAsync(product, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static AdminProduct ApiAdminProductsByIdGet(this IAdminAdventureAPI operations, System.Guid id)
            {
                return operations.ApiAdminProductsByIdGetAsync(id).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<AdminProduct> ApiAdminProductsByIdGetAsync(this IAdminAdventureAPI operations, System.Guid id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiAdminProductsByIdGetWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='product'>
            /// </param>
            public static IDictionary<string, ModelStateEntry> ApiAdminProductsByIdPut(this IAdminAdventureAPI operations, System.Guid id, UpdateProduct product = default(UpdateProduct))
            {
                return operations.ApiAdminProductsByIdPutAsync(id, product).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='product'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IDictionary<string, ModelStateEntry>> ApiAdminProductsByIdPutAsync(this IAdminAdventureAPI operations, System.Guid id, UpdateProduct product = default(UpdateProduct), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiAdminProductsByIdPutWithHttpMessagesAsync(id, product, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static void ApiAdminProductsByIdDelete(this IAdminAdventureAPI operations, System.Guid id)
            {
                operations.ApiAdminProductsByIdDeleteAsync(id).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task ApiAdminProductsByIdDeleteAsync(this IAdminAdventureAPI operations, System.Guid id, CancellationToken cancellationToken = default(CancellationToken))
            {
                (await operations.ApiAdminProductsByIdDeleteWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false)).Dispose();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='patchProduct'>
            /// </param>
            public static IDictionary<string, ModelStateEntry> ApiAdminProductsByIdPatch(this IAdminAdventureAPI operations, System.Guid id, IList<Operation> patchProduct = default(IList<Operation>))
            {
                return operations.ApiAdminProductsByIdPatchAsync(id, patchProduct).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='patchProduct'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IDictionary<string, ModelStateEntry>> ApiAdminProductsByIdPatchAsync(this IAdminAdventureAPI operations, System.Guid id, IList<Operation> patchProduct = default(IList<Operation>), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ApiAdminProductsByIdPatchWithHttpMessagesAsync(id, patchProduct, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}