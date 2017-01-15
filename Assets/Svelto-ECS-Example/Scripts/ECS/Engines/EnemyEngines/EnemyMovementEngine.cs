using Svelto.ECS.Example.Nodes.Enemies;
using Svelto.Ticker.Legacy;
using System;
using UnityEngine;

namespace Svelto.ECS.Example.Engines.Enemies
{
    public class EnemyMovementEngine : INodesEngine, ITickable, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }

        public Type[] AcceptedNodes()
        {
            return _acceptedNodes;
        }

        public void Add(INode obj)
        {
            if (obj is EnemyNode)
            {
                var enemyNode = obj as EnemyNode;
                var healthEventsComponent = enemyNode.healthComponent;

                healthEventsComponent.isDead.NotifyOnDataChange(StopEnemyOnDeath);
            }
            else
                _targetNode = obj as EnemyTargetNode;
        }

        public void Remove(INode obj)
        {
            if (obj is EnemyNode)
            {
                var enemyNode = obj as EnemyNode;
                var healthEventsComponent = enemyNode.healthComponent;

                healthEventsComponent.isDead.StopNotifyOnDataChange(StopEnemyOnDeath);
            }
            else
                _targetNode = null;
        }

        public void Tick(float deltaSec)
        {
            if (_targetNode == null)
                return;
            
            var enemies = nodesDB.QueryNodes<EnemyNode>();

            for (var i = 0; i < enemies.Count; i++)
            {
                var component = enemies[i].movementComponent;

                if (component.navMesh.isActiveAndEnabled)
                    component.navMesh.SetDestination(_targetNode.targetPositionComponent.position);
            }
        }

        void StopEnemyOnDeath(int targetID, bool isDead)
        {
            EnemyNode node = nodesDB.QueryNode<EnemyNode>(targetID);

            node.movementComponent.navMesh.enabled = false;
            var capsuleCollider = node.movementComponent.capsuleCollider;
            capsuleCollider.isTrigger = true;
            capsuleCollider.GetComponent<Rigidbody>().isKinematic = true;
        }

        readonly Type[] _acceptedNodes = { typeof(EnemyNode), typeof(EnemyTargetNode) };

        EnemyTargetNode   _targetNode;
    }
}
