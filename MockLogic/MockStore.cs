using MockLogic.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MockLogic
{
    public static class MockStore
    {
        private static readonly ConcurrentDictionary<Guid, Mock> mockDictionary;
        private static readonly ConcurrentDictionary<Request, Guid> requestIndex;

        static MockStore()
        {
            mockDictionary = new ConcurrentDictionary<Guid, Mock>();
            requestIndex = new ConcurrentDictionary<Request, Guid>();
        }

        public static void CreateOrUpdate(Mock mock)
        {
            mockDictionary.AddOrUpdate(mock.Reference, mock, (request, oldMock) => mock);
            requestIndex.TryAdd(mock.Request, mock.Reference);
        }

        public static void Delete(Guid mockReference)
        {
            mockDictionary.TryRemove(mockReference, out var mock);
            if (mock != null)
            {
                requestIndex.TryRemove(mock.Request, out _);
            }
        }

        public static Mock Retrieve(Request mockRequest)
        {
            if (!requestIndex.TryGetValue(mockRequest, out var mockReference))
            {
                return null;
            }

            return Retrieve(mockReference);
        }

        public static Mock Retrieve(Guid mockReference)
        {
            if (!mockDictionary.TryGetValue(mockReference, out var response))
            {
                return null;
            }

            return response;
        }

        public static ICollection<Mock> RetrieveAll()
        {
            return mockDictionary.Values;
        }
    }
}
