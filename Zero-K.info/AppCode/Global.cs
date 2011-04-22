﻿using System;
using System.Web;
using System.Web.Mvc;
using ZkData;

namespace ZeroKWeb
{
	public static class Global
	{
		public const int AjaxScrollCount = 40;
		public static Account Account { get { return HttpContext.Current.User as Account; } }
		public static int AccountID
		{
			get
			{
				if (IsAccountAuthorized) return Account.AccountID;
				else return 0;
			}
		}

		public static int ClanID
		{
			get
			{
				if (IsAccountAuthorized && Clan != null) return Clan.ClanID;
				else return 0;
			} 
		}

		public static Clan Clan
		{
			get
			{
				if (Account == null) return null;
				else return Account.Clan;
			}
		}

		public static bool IsAccountAuthorized { get { return HttpContext.Current.User as Account != null; } }

		public static bool IsLimitedMode
		{
			get
			{
				if (HttpContext.Current.Request[GlobalConst.LimitedModeCookieName] == "0") return false;
				var cook = HttpContext.Current.Request.Cookies[GlobalConst.LimitedModeCookieName];
				if (cook != null && cook.Value == "0") return false;
				return true;
			}
		}
		public static bool IsLobbyAccess { get { return HttpContext.Current.Request.Cookies[GlobalConst.LobbyAccessCookieName] != null; } }
		public static bool IsLobbyAdmin { get { return IsAccountAuthorized && Account.IsLobbyAdministrator; } }
		public static bool IsZeroKAdmin { get { return IsAccountAuthorized && Account.IsZeroKAdmin; } }

		public static Event CreateEvent(string format, params object[] args)
		{
			var ev = new Event() { Time = DateTime.UtcNow };

			for (int i = 0; i < args.Length; i++) {
				var arg = args[i];
				var url = new UrlHelper(HttpContext.Current.Request.RequestContext);

				if (arg is Account) {
					var acc = (Account)arg;
					args[i] = HtmlHelperExtensions.PrintAccount(null, acc, false);
					if (acc.AccountID != 0) ev.EventAccounts.Add(new EventAccount() { AccountID =  acc.AccountID });
					else ev.EventAccounts.Add(new EventAccount() { Account = acc});
				} else if (arg is Clan)
				{
					var clan = (Clan)arg;
					args[i] = HtmlHelperExtensions.PrintClan(null, clan, false);
					if (clan.ClanID != 0) ev.EventClans.Add(new EventClan() { ClanID = clan.ClanID});
					else ev.EventClans.Add(new EventClan() { Clan = clan}); 
				} else if (arg is Planet)
				{
					var planet = (Planet)arg;
					args[i] = HtmlHelperExtensions.PrintPlanet(null, planet);
					if (planet.PlanetID != 0) ev.EventPlanets.Add(new EventPlanet() { PlanetID = planet.PlanetID });
					else ev.EventPlanets.Add(new EventPlanet() { Planet = planet });
				} else if (arg is SpringBattle) {
					var bat = (SpringBattle)arg;
					args[i] = string.Format("<a href='{0}'>B{1}</a>", url.Action("Detail", "Battles", new { id = bat.SpringBattleID }));   //todo no propoer helper for this
					if (bat.SpringBattleID != 0) ev.EventSpringBattles.Add(new EventSpringBattle() { SpringBattleID = bat.SpringBattleID });
					else ev.EventSpringBattles.Add(new EventSpringBattle() { SpringBattle = bat });
				}

			}
			ev.Text = string.Format(format, args);
			return ev;
		}

	}
}