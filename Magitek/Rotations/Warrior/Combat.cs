using System.Threading.Tasks;
using ff14bot;
using ff14bot.Managers;
using Magitek.Extensions;
using Magitek.Logic;
using Magitek.Logic.Roles;
using Magitek.Logic.Warrior;
using Magitek.Models.Warrior;
using Magitek.Utilities;

namespace Magitek.Rotations.Warrior
{
    internal static class Combat
    {
        public static async Task<bool> Execute()
        {
            if (!Core.Me.HasTarget || !Core.Me.CurrentTarget.ThoroughCanAttack())
                return false;

            if (await CustomOpenerLogic.Opener()) return true;

            if (BotManager.Current.IsAutonomous)
            {          
                Movement.NavigateToUnitLos(Core.Me.CurrentTarget, 4);
            }

            if (await SingleTarget.LowBlow()) return true;
            if (await SingleTarget.Interject()) return true;
            if (await Buff.Stance()) return true;

            if (Utilities.Routines.Warrior.OnGcd)
            {
                if (await Defensive.ExecuteTankBusters()) return true;
                if (await Defensive.Defensives()) return true;
            }

            if (WarriorSettings.Instance.IsMainTank)
            {
                if (await Tank.Provoke(WarriorSettings.Instance)) return true;
                if (await SingleTarget.TomahawkOnLostAggro()) return true;
                if (await Buff.InnerReleaseMainTank()) return true;
                if (await Aoe.SteelCyclone()) return true;
                if (await SingleTarget.InnerBeast()) return true;
            }
            else
            {
                if (await Buff.InnerReleaseOffTank()) return true;
                if (await SingleTarget.InnerReleaseFellCleaveSpam()) return true;
                if (await Aoe.Decimate()) return true;
                if (await SingleTarget.FellCleave()) return true;
            }

            if (await SingleTarget.Onslaught()) return true;
            if (await Buff.Beserk()) return true;
            if (await Buff.Infuriate()) return true;
            if (await SingleTarget.Upheaval()) return true;
            if (await Aoe.Overpower()) return true;

            // Main Rotation Part

            if (await SingleTarget.StormsEye()) return true;         
            if (await SingleTarget.StormsPath()) return true;
            if (await SingleTarget.Maim()) return true;
            if (await SingleTarget.HeavySwing()) return true;
            return await SingleTarget.Tomahawk();

        }
    }
}