﻿namespace DigitalRepository.Server.Interceptors.Interfaces
{
    using FluentValidation.Results;
    using Entities.Response;

    /// <summary>
    /// Defines the <see cref="IEntityAfterPartialUpdateInterceptor{T, in TRequest}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    public interface IEntityAfterPartialUpdateInterceptor<T, in TRequest>
    {
        /// <summary>
        /// The Execute
        /// </summary>
        /// <param name="response">The response<see cref="Response{T, List{ValidationFailure}}"/></param>
        /// <param name="request">The request<see cref="TRequest"/></param>
        /// <param name="prevState">The prevState<see cref="T"/></param>
        /// <returns>The <see cref="Response{T, List{ValidationFailure}}"/></returns>
        Response<T, List<ValidationFailure>> Execute(Response<T, List<ValidationFailure>> response, TRequest request, T prevState);
    }
}
