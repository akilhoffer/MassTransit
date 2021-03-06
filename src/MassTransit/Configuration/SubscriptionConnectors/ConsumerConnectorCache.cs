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
namespace MassTransit.SubscriptionConnectors
{
	using System;
	using System.Collections.Generic;
	using Configuration;
	using Magnum.Extensions;
	using Magnum.Reflection;

	public class ConsumerConnectorCache
	{
		[ThreadStatic]
		static ConsumerConnectorCache _current;

		readonly IDictionary<Type, ConsumerConnector> _connectors;

		ConsumerConnectorCache()
		{
			_connectors = new Dictionary<Type, ConsumerConnector>();
		}

		static ConsumerConnectorCache Instance
		{
			get
			{
				if (_current == null)
					_current = new ConsumerConnectorCache();

				return _current;
			}
		}

		public static ConsumerConnector GetConsumerConnector<T>(IConsumerFactory<T> consumerFactory)
			where T : class
		{
			return Instance._connectors.Retrieve(typeof (T), () => new ConsumerConnector<T>(consumerFactory));
		}

		public static ConsumerConnector GetConsumerConnector(Type type, Func<Type, object> consumerFactory)
		{
			return Instance._connectors.Retrieve(type, () => { return ConsumerConnectorFactory(type, consumerFactory); });
		}

		static ConsumerConnector ConsumerConnectorFactory(Type type, Func<Type, object> consumerFactory)
		{
			object factory = FastActivator.Create(typeof (ObjectConsumerFactory<>), new[] {type},
				new object[] {consumerFactory});

			var args = new[] {factory};
			var connector = (ConsumerConnector) FastActivator.Create(typeof (ConsumerConnector<>), new[] {type}, args);

			return connector;
		}
	}
}