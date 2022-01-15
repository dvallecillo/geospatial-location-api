﻿using System;

namespace GeospatialLocation.Application.Constants
{
    public static class LocationConstants
    {
        public const int ClusterRange = 10000; // 10KM
        public const double MinLat = -90;
        public const double MaxLat = 90;
        public const double MinLon = -180;
        public const double MaxLon = 180;
        public static double ClusterDiagonal = Math.Sqrt(2) * ClusterRange; // 10KM
    }
}