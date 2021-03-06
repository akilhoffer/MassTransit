﻿// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.SubscriptionConfigurators
{
	using System;
	using Subscriptions;

	public class SubscriptionConfiguratorImpl<TInterface> :
		SubscriptionConfigurator<TInterface>
		where TInterface : class, SubscriptionConfigurator<TInterface>
	{
		Func<UnsubscribeAction, ISubscriptionReference> _referenceFactory;

		protected SubscriptionConfiguratorImpl()
		{
			Transient();
		}

		protected Func<UnsubscribeAction, ISubscriptionReference> ReferenceFactory
		{
			get { return _referenceFactory; }
		}

		public TInterface Permanent()
		{
			_referenceFactory = PermanentSubscriptionReference.Create;

			return this as TInterface;
		}

		public TInterface Transient()
		{
			_referenceFactory = TransientSubscriptionReference.Create;

			return this as TInterface;
		}

		public TInterface SetReferenceFactory(Func<UnsubscribeAction, ISubscriptionReference> referenceFactory)
		{
			_referenceFactory = referenceFactory;

			return this as TInterface;
		}
	}
}