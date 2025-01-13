using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace GamePlay.StageData.Player.PathFinder
{
    public static class Dijkstra
    {
        private static readonly IEnumerable<Direction> MovingDirections = new[]
        {
            Direction.Up,
            Direction.Down,
            Direction.Left,
            Direction.Right,
        };

        [CanBeNull]
        public static List<Coordinates> GetPath(StageElement[] elements, Coordinates from, Coordinates to)
        {
            // 1) 갈 수 있는 길(일반 타일)
            var walkableSet = elements
                .Where(e => e.Type == StageElementType.Tile)
                .Select(e => e.Coordinates)
                .ToHashSet();

            // 2) 해비 길(heavy path)
            var heavySet = elements
                .Where(e => e.Type == StageElementType.Speaker)
                .Select(e => e.Coordinates)
                .ToHashSet();

            // 3) (거리, 해비) 비용 맵
            var costMap = new Dictionary<Coordinates, (int dist, int heavy)>();
            costMap[from] = (0, 0);

            // 4) 다익스트라 우선순위 큐 (dist → heavy) 오름차순
            var openSet = new SortedSet<PathNode>(new PathNodeComparer());
            openSet.Add(new PathNode(from, 0, 0));

            // 5) 경로 복원을 위한 parentMap
            var parentMap = new Dictionary<Coordinates, Coordinates>();

            // 6) 방문 여부(확정 비용)
            var visited = new HashSet<Coordinates>();

            while (openSet.Count > 0)
            {
                // (A) 가장 비용이 작은 노드를 꺼낸다
                var currentNode = openSet.Min;
                openSet.Remove(currentNode);

                var currentCoord = currentNode.Coord;
                var (currDist, currHeavy) = (currentNode.Dist, currentNode.Heavy);

                // (B) 이미 방문된 노드(확정 비용)면 건너뛴다
                if (visited.Contains(currentCoord))
                    continue;

                // (C) 혹시 costMap에 더 나은 경로가 있어서 갱신되어버린 상태라면, 이 노드는 구버전이므로 버림
                //     (ex. openSet에 중복으로 들어가 있을 수 있음)
                if (!costMap.ContainsKey(currentCoord) ||
                    costMap[currentCoord].dist != currDist ||
                    costMap[currentCoord].heavy != currHeavy)
                    continue;

                // (D) 드디어 확정 처리
                visited.Add(currentCoord);

                // 목표에 도달했으면 경로 복원
                if (currentCoord.Equals(to))
                    return ReconstructPath(parentMap, from, to);

                // (E) 인접 노드 탐색
                foreach (var dir in MovingDirections)
                {
                    var neighbor = currentCoord + dir;

                    // walkableSet 또는 heavySet에 속해야 "갈 수 있는 길"이라고 가정
                    if (!walkableSet.Contains(neighbor) && !heavySet.Contains(neighbor))
                        continue;

                    var newDist = currDist + 1;
                    var newHeavy = currHeavy + (heavySet.Contains(neighbor) ? 1 : 0);

                    // 만약 neighbor가 아직 costMap에 없거나, 더 좋은 비용을 찾았다면 갱신
                    if (!costMap.ContainsKey(neighbor) 
                        || IsBetter((newDist, newHeavy), costMap[neighbor]))
                    {
                        costMap[neighbor] = (newDist, newHeavy);
                        parentMap[neighbor] = currentCoord;

                        // 방문 확정된 노드는 굳이 또 넣을 필요 없으나
                        // 아직 확정되지 않았다면 openSet에 새로 넣음
                        if (!visited.Contains(neighbor))
                        {
                            openSet.Add(new PathNode(neighbor, newDist, newHeavy));
                        }
                    }
                }
            }

            // 경로가 없다
            return null;
        }

        private static bool IsBetter((int dist, int heavy) candidate, (int dist, int heavy) current)
        {
            // 1) 거리 비교
            if (candidate.dist < current.dist) return true;
            // 2) 거리 같을 땐 해비 타일 수 비교
            if (candidate.dist == current.dist && candidate.heavy < current.heavy) return true;
            return false;
        }

        private static List<Coordinates> ReconstructPath(
            Dictionary<Coordinates, Coordinates> parentMap,
            Coordinates start,
            Coordinates end)
        {
            var path = new List<Coordinates>();
            var cur = end;

            while (!cur.Equals(start))
            {
                path.Add(cur);
                cur = parentMap[cur];
            }
            path.Add(start);
            path.Reverse();
            return path;
        }

        private class PathNode
        {
            public Coordinates Coord { get; }
            public int Dist { get; }
            public int Heavy { get; }

            public PathNode(Coordinates c, int dist, int heavy)
            {
                Coord = c;
                Dist = dist;
                Heavy = heavy;
            }
        }

        private class PathNodeComparer : IComparer<PathNode>
        {
            public int Compare(PathNode x, PathNode y)
            {
                // 거리 먼저
                var distCompare = x.Dist.CompareTo(y.Dist);
                if (distCompare != 0)
                    return distCompare;

                // 거리 같으면 해비 타일 수
                var heavyCompare = x.Heavy.CompareTo(y.Heavy);
                if (heavyCompare != 0)
                    return heavyCompare;

                // 모든 게 같으면 좌표 비교
                // 좌표까지 완전히 같으면 0
                // (SortedSet에서 0이면 “동일 노드”로 처리)
                return x.Coord.CompareTo(y.Coord); 
            }
        }
    }
}
