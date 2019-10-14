using BattleSimulator.Services.Requests;
using BattleSimulator.Services.Responses;
using BattleSimulator.Services.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BattleSimulator.Services.Pipelines
{
    public class ProcessingPipeline<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : RequestBase, IRequest<TResponse> where TResponse : ResponseBase, new()
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly ILogger<ProcessingPipeline<TRequest, TResponse>> _logger;

        public ProcessingPipeline(IRequestHandler<TRequest, TResponse> inner, ILogger<ProcessingPipeline<TRequest, TResponse>> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var response = new TResponse();
            try
            {
                response = await _inner.Handle(request, cancellationToken);
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add($"An exception occured while processing your request: {ex.Message}");

                return response;
            }
            finally
            {
                if (response.HasErrors)
                {
                    _logger.LogError($"An error(s) have occured {response.ErrorMessages.GetErrorMessagesFormated()}");
                }
            }
        }
    }
}
