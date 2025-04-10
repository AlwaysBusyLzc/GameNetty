
namespace ET.Server
{
	[MessageHandler(SceneType.Map)]
	public class C2M_PathfindingResultHandler : MessageHandler<Unit, C2M_PathfindingResult>
	{
		protected override async ETTask Run(Unit unit, C2M_PathfindingResult message)
		{
			unit.FindPathMoveToAsync(message.Position.ToFloat3()).Coroutine();
			await ETTask.CompletedTask;
		}
	}
}