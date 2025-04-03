using Unity.Mathematics;

namespace ET.Server
{
    [Event(SceneType.Map)]
    public class ChangePosition_NotifyAOI: AEvent<Scene, ChangePosition>
    {
        protected override async ETTask Run(Scene scene, ChangePosition args)
        {
            Unit unit = args.Unit;
            float3 oldPos = args.OldPos;
            int oldCellX = (int) (oldPos.X * 1000) / AOIManagerComponent.CellSize;
            int oldCellY = (int) (oldPos.Z * 1000) / AOIManagerComponent.CellSize;
            int newCellX = (int) (unit.Position.X * 1000) / AOIManagerComponent.CellSize;
            int newCellY = (int) (unit.Position.Z * 1000) / AOIManagerComponent.CellSize;
            if (oldCellX == newCellX && oldCellY == newCellY)
            {
                return;
            }

            AOIEntity aoiEntity = unit.GetComponent<AOIEntity>();
            if (aoiEntity == null)
            {
                return;
            }

            unit.Scene().GetComponent<AOIManagerComponent>().Move(aoiEntity, newCellX, newCellY);
            await ETTask.CompletedTask;
        }
    }
}