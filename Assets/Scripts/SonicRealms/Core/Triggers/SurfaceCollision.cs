﻿using SonicRealms.Core.Actors;
using SonicRealms.Core.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SonicRealms.Core.Triggers
{
    /// <summary>
    /// Stores surface collision info for PlatformTriggers and ReactivePlatforms.
    /// </summary>
    public struct SurfaceCollision
    {
        private readonly Contact[] _contacts;

        /// <summary>
        /// The controller that collided with the surface.
        /// </summary>
        public HedgehogController Controller
        {
            get { return _contacts.Length > 0 ? _contacts[0].HitData.Controller : null; }
        }

        /// <summary>
        /// The number of contact points in the collision.
        /// </summary>
        public int Count { get { return _contacts != null ? _contacts.Length : 0; } }

        /// <summary>
        /// Gets the contact point at the given index. Use Count to find out how many contacts there are.
        /// </summary>
        /// <returns></returns>
        public Contact this[int index] { get { return _contacts[index]; } }

        /// <summary>
        /// Gets the latest contact point that contains the ground sensor with the given side, Left or Right.
        /// Passing in GroundSensorType.Both will return the latest contact.
        /// </summary>
        public Contact this[GroundSensorType sensor]
        {
            get
            {
                if (_contacts == null)
                    return default(Contact);

                for (var i = _contacts.Length - 1; i >= 0; --i)
                {
                    var c = _contacts[i];

                    if (sensor.Has(c.Sensor))
                        return c;
                }

                return default(Contact);
            }
        }

        /// <summary>
        /// The contact point that was registered last.
        /// </summary>
        public Contact Latest { get { return this[GroundSensorType.Both]; } }

        /// <summary>
        /// The latest contact between the controller's left ground sensor and the surface, if any.
        /// </summary>
        public Contact Left { get { return this[GroundSensorType.Left]; } }

        /// <summary>
        /// The latest contact between the controller's right ground sensor and the surface, if any.
        /// </summary>
        public Contact Right { get { return this[GroundSensorType.Right]; } }

        public SurfaceCollision(IEnumerable<Contact> contacts)
        {
            _contacts = contacts.ToArray();
        }
        
        /// <summary>
        /// Returns true if the collision has any contacts, false otherwise.
        /// </summary>
        public static implicit operator bool (SurfaceCollision collision)
        {
            return collision._contacts != null && collision._contacts.Length > 0;
        }

        /// <summary>
        /// A single instance of surface collision, of which many may make up SurfaceCollision.
        /// </summary>
        public struct Contact
        {
            /// <summary>
            /// Indicates whether the Left or Right ground sensor registered the contact.
            /// </summary>
            public readonly GroundSensorType Sensor;

            /// <summary>
            /// Raycast hit and geometry data.
            /// </summary>
            public readonly TerrainCastHit HitData;

            /// <summary>
            /// The controller that came into contact with the platform's surface.
            /// </summary>
            public HedgehogController Controller { get { return HitData ? HitData.Controller : null; } }

            /// <summary>
            /// The platform trigger that came into contact with the controller, if any.
            /// </summary>
            public readonly PlatformTrigger PlatformTrigger;

            /// <summary>
            /// The controller's velocity at the time of contact.
            /// </summary>
            public readonly Vector2 Velocity;

            /// <summary>
            /// The controller's velocity relative to its gravity at the time of contact.
            /// </summary>
            public readonly Vector2 RelativeVelocity;

            /// <summary>
            /// The controller's ground speed at the time of contact.
            /// </summary>
            public readonly float GroundVelocity;

            /// <summary>
            /// The controller's surface angle at the time of contact.
            /// </summary>
            public readonly float SurfaceAngle;

            /// <summary>
            /// The controller's surface angle relative to its gravity at the time of contact.
            /// </summary>
            public readonly float RelativeSurfaceAngle;

            public Contact(GroundSensorType sensor, TerrainCastHit hitData, PlatformTrigger platformTrigger = null)
            {
                PlatformTrigger = platformTrigger;
                Sensor = sensor;
                HitData = hitData;

                Velocity = hitData.Controller.Velocity;
                RelativeVelocity = hitData.Controller.RelativeVelocity;
                GroundVelocity = hitData.Controller.GroundVelocity;
                SurfaceAngle = hitData.Controller.SurfaceAngle;
                RelativeSurfaceAngle = hitData.Controller.RelativeSurfaceAngle;
            }

            /// <summary>
            /// Returns true if the contact has any data, false otherwise.
            /// </summary>
            public static implicit operator bool (Contact contact)
            {
                return contact.HitData;
            }
        }
    }
}