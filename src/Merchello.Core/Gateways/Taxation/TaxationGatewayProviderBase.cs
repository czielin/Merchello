﻿using System;
using System.Collections.Generic;
using Merchello.Core.Models;
using Merchello.Core.Services;
using Umbraco.Core.Cache;

namespace Merchello.Core.Gateways.Taxation
{
    /// <summary>
    /// Defines a base taxation gateway provider
    /// </summary>
    public abstract class TaxationGatewayProviderBase : GatewayProviderBase, ITaxationGatewayProvider
    {
        

        protected TaxationGatewayProviderBase(IGatewayProviderService gatewayProviderService, IGatewayProvider gatewayProvider, IRuntimeCacheProvider runtimeCacheProvider) 
            : base(gatewayProviderService, gatewayProvider, runtimeCacheProvider)
        { }

        /// <summary>
        /// Calculates the tax amount for an invoice
        /// </summary>
        /// <param name="invoice"><see cref="IInvoice"/></param>
        /// <returns><see cref="IInvoiceTaxResult"/></returns>
        /// <remarks>
        /// 
        /// Assumes the billing address of the invoice will be used for the taxation address
        /// 
        /// </remarks>
        public virtual IInvoiceTaxResult CalculateTaxForInvoice(IInvoice invoice)
        {
            return CalculateTaxForInvoice(invoice, invoice.GetBillingAddress());
        }

        /// <summary>
        /// Calculates the tax amount for an invoice
        /// </summary>
        /// <param name="invoice"><see cref="IInvoice"/></param>
        /// <param name="taxAddress">The <see cref="IAddress"/> to base taxation rates.  Either origin or destination address.</param>
        /// <returns><see cref="IInvoiceTaxResult"/></returns>
        public abstract IInvoiceTaxResult CalculateTaxForInvoice(IInvoice invoice, IAddress taxAddress);


        /// <summary>
        /// Calculates the tax amount for an invoice
        /// </summary>
        /// <param name="strategy">The invoice taxation strategy to use when calculating the tax amount</param>
        /// <returns><see cref="IInvoiceTaxResult"/></returns>
        public virtual IInvoiceTaxResult CalculateTaxForInvoice(IInvoiceTaxationStrategy strategy)
        {
            var attempt = strategy.CalculateTaxesForInvoice();

            if (!attempt.Success) throw attempt.Exception;

            return attempt.Result;
        }


        private IEnumerable<ITaxMethod> _taxMethods;
        /// <summary>
        /// Gets a collection of <see cref="ITaxMethod"/> assoicated with this provider
        /// </summary>
        public IEnumerable<ITaxMethod> TaxMethods
        {
            get {
                return _taxMethods ??
                       (_taxMethods = GatewayProviderService.GetTaxMethodsByProviderKey(GatewayProvider.Key));
            }
        }
    }
}