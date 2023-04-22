using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using Verse;

namespace MSS.MemeSuperpack;

public class PatchOperationToggleable : PatchOperation
{
	private static Lazy<Dictionary<string, FieldInfo>> settings = new(() =>
		typeof(Settings).GetFields().ToDictionary(f => f.Name));

	public string setting;
	public PatchOperation enabled;
	public PatchOperation disabled;

	protected override bool ApplyWorker(XmlDocument xml)
	{
		FieldInfo settingField = settings.Value.GetWithFallback(setting);

		if (settingField == null)
		{
			Verse.Log.Error(
				$"Setting {setting} not a valid setting. Valid options are: [{settings.Value.Keys.ToCommaList()}]");
			return false;
		}

		if (settingField.GetValue(MemeSuperpackMod.settings) is bool active)
		{
			PatchOperation patchOperation = active ? enabled : disabled;
			return patchOperation?.Apply(xml) ?? true;
		}

		Verse.Log.Error($"Setting {setting} could not be read as a bool");
		return false;
	}
}
