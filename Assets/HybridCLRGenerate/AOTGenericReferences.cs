using System.Collections.Generic;
public class AOTGenericReferences : UnityEngine.MonoBehaviour
{

	// {{ AOT assemblies
	public static readonly IReadOnlyList<string> PatchedAOTAssemblyList = new List<string>
	{
		"Google.Protobuf.dll",
		"System.Core.dll",
		"System.dll",
		"UnityEngine.CoreModule.dll",
		"mscorlib.dll",
	};
	// }}

	// {{ constraint implement type
	// }} 

	// {{ AOT generic types
	// Google.Protobuf.Collections.MapField.<>c<long,object>
	// Google.Protobuf.Collections.MapField.<>c__DisplayClass7_0<long,object>
	// Google.Protobuf.Collections.MapField.Codec<long,object>
	// Google.Protobuf.Collections.MapField.DictionaryEnumerator<long,object>
	// Google.Protobuf.Collections.MapField.MapView<long,object,long>
	// Google.Protobuf.Collections.MapField.MapView<long,object,object>
	// Google.Protobuf.Collections.MapField<long,object>
	// Google.Protobuf.Collections.RepeatedField.<GetEnumerator>d__28<object>
	// Google.Protobuf.Collections.RepeatedField<object>
	// Google.Protobuf.FieldCodec.<>c<long>
	// Google.Protobuf.FieldCodec.<>c<object>
	// Google.Protobuf.FieldCodec.<>c__32<object>
	// Google.Protobuf.FieldCodec.<>c__DisplayClass32_0<object>
	// Google.Protobuf.FieldCodec.<>c__DisplayClass38_0<long>
	// Google.Protobuf.FieldCodec.<>c__DisplayClass38_0<object>
	// Google.Protobuf.FieldCodec.<>c__DisplayClass39_0<long>
	// Google.Protobuf.FieldCodec.<>c__DisplayClass39_0<object>
	// Google.Protobuf.FieldCodec.InputMerger<long>
	// Google.Protobuf.FieldCodec.InputMerger<object>
	// Google.Protobuf.FieldCodec.ValuesMerger<long>
	// Google.Protobuf.FieldCodec.ValuesMerger<object>
	// Google.Protobuf.FieldCodec<long>
	// Google.Protobuf.FieldCodec<object>
	// Google.Protobuf.IDeepCloneable<object>
	// Google.Protobuf.IMessage<object>
	// Google.Protobuf.MessageParser.<>c__DisplayClass2_0<object>
	// Google.Protobuf.MessageParser<object>
	// Google.Protobuf.ValueReader<long>
	// Google.Protobuf.ValueReader<object>
	// Google.Protobuf.ValueWriter<long>
	// Google.Protobuf.ValueWriter<object>
	// System.Action<System.ArraySegment<byte>>
	// System.Action<byte,object>
	// System.Action<kcp2k.AckItem>
	// System.Action<object,UnityEngine.Color32>
	// System.Action<object,int>
	// System.Action<object,object,object,object>
	// System.Action<object,object,object>
	// System.Action<object,object>
	// System.Action<object>
	// System.ArraySegment.Enumerator<byte>
	// System.ArraySegment.Enumerator<object>
	// System.ArraySegment<byte>
	// System.ArraySegment<object>
	// System.ByReference<byte>
	// System.Collections.Concurrent.ConcurrentQueue.<Enumerate>d__28<object>
	// System.Collections.Concurrent.ConcurrentQueue.Segment<object>
	// System.Collections.Concurrent.ConcurrentQueue<object>
	// System.Collections.Generic.ArraySortHelper<kcp2k.AckItem>
	// System.Collections.Generic.ArraySortHelper<object>
	// System.Collections.Generic.Comparer<double>
	// System.Collections.Generic.Comparer<kcp2k.AckItem>
	// System.Collections.Generic.Comparer<object>
	// System.Collections.Generic.Dictionary.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.KeyCollection<int,object>
	// System.Collections.Generic.Dictionary.KeyCollection<long,object>
	// System.Collections.Generic.Dictionary.KeyCollection<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection.Enumerator<object,object>
	// System.Collections.Generic.Dictionary.ValueCollection<int,object>
	// System.Collections.Generic.Dictionary.ValueCollection<long,object>
	// System.Collections.Generic.Dictionary.ValueCollection<object,object>
	// System.Collections.Generic.Dictionary<int,object>
	// System.Collections.Generic.Dictionary<long,object>
	// System.Collections.Generic.Dictionary<object,object>
	// System.Collections.Generic.EqualityComparer<Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.EqualityComparer<Network.Sync.TransformSnapshot>
	// System.Collections.Generic.EqualityComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.EqualityComparer<double>
	// System.Collections.Generic.EqualityComparer<float>
	// System.Collections.Generic.EqualityComparer<int>
	// System.Collections.Generic.EqualityComparer<long>
	// System.Collections.Generic.EqualityComparer<object>
	// System.Collections.Generic.ICollection<Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.ICollection<Network.Sync.TransformSnapshot>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<double,Common.Tools.SnapshotInterpolation.TimeSnapshot>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<double,Network.Sync.TransformSnapshot>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<double,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.ICollection<double>
	// System.Collections.Generic.ICollection<kcp2k.AckItem>
	// System.Collections.Generic.ICollection<object>
	// System.Collections.Generic.IComparer<double>
	// System.Collections.Generic.IComparer<kcp2k.AckItem>
	// System.Collections.Generic.IComparer<object>
	// System.Collections.Generic.IDictionary<double,Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.IDictionary<double,Network.Sync.TransformSnapshot>
	// System.Collections.Generic.IDictionary<double,object>
	// System.Collections.Generic.IDictionary<object,object>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerable<kcp2k.AckItem>
	// System.Collections.Generic.IEnumerable<long>
	// System.Collections.Generic.IEnumerable<object>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<int,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.IEnumerator<System.Collections.Generic.KeyValuePair<object,object>>
	// System.Collections.Generic.IEnumerator<kcp2k.AckItem>
	// System.Collections.Generic.IEnumerator<long>
	// System.Collections.Generic.IEnumerator<object>
	// System.Collections.Generic.IEqualityComparer<int>
	// System.Collections.Generic.IEqualityComparer<long>
	// System.Collections.Generic.IEqualityComparer<object>
	// System.Collections.Generic.IList<Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.IList<Network.Sync.TransformSnapshot>
	// System.Collections.Generic.IList<double>
	// System.Collections.Generic.IList<kcp2k.AckItem>
	// System.Collections.Generic.IList<object>
	// System.Collections.Generic.KeyValuePair<double,Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.KeyValuePair<double,Network.Sync.TransformSnapshot>
	// System.Collections.Generic.KeyValuePair<double,object>
	// System.Collections.Generic.KeyValuePair<int,object>
	// System.Collections.Generic.KeyValuePair<long,object>
	// System.Collections.Generic.KeyValuePair<object,object>
	// System.Collections.Generic.LinkedList.Enumerator<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.LinkedList<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.LinkedListNode<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.List.Enumerator<kcp2k.AckItem>
	// System.Collections.Generic.List.Enumerator<object>
	// System.Collections.Generic.List<kcp2k.AckItem>
	// System.Collections.Generic.List<object>
	// System.Collections.Generic.ObjectComparer<double>
	// System.Collections.Generic.ObjectComparer<kcp2k.AckItem>
	// System.Collections.Generic.ObjectComparer<object>
	// System.Collections.Generic.ObjectEqualityComparer<Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.ObjectEqualityComparer<Network.Sync.TransformSnapshot>
	// System.Collections.Generic.ObjectEqualityComparer<System.Collections.Generic.KeyValuePair<long,object>>
	// System.Collections.Generic.ObjectEqualityComparer<double>
	// System.Collections.Generic.ObjectEqualityComparer<float>
	// System.Collections.Generic.ObjectEqualityComparer<int>
	// System.Collections.Generic.ObjectEqualityComparer<long>
	// System.Collections.Generic.ObjectEqualityComparer<object>
	// System.Collections.Generic.Queue.Enumerator<object>
	// System.Collections.Generic.Queue<object>
	// System.Collections.Generic.SortedList.Enumerator<double,Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.SortedList.Enumerator<double,Network.Sync.TransformSnapshot>
	// System.Collections.Generic.SortedList.Enumerator<double,object>
	// System.Collections.Generic.SortedList.Enumerator<object,object>
	// System.Collections.Generic.SortedList.KeyList<double,Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.SortedList.KeyList<double,Network.Sync.TransformSnapshot>
	// System.Collections.Generic.SortedList.KeyList<double,object>
	// System.Collections.Generic.SortedList.KeyList<object,object>
	// System.Collections.Generic.SortedList.SortedListKeyEnumerator<double,Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.SortedList.SortedListKeyEnumerator<double,Network.Sync.TransformSnapshot>
	// System.Collections.Generic.SortedList.SortedListKeyEnumerator<double,object>
	// System.Collections.Generic.SortedList.SortedListKeyEnumerator<object,object>
	// System.Collections.Generic.SortedList.SortedListValueEnumerator<double,Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.SortedList.SortedListValueEnumerator<double,Network.Sync.TransformSnapshot>
	// System.Collections.Generic.SortedList.SortedListValueEnumerator<double,object>
	// System.Collections.Generic.SortedList.SortedListValueEnumerator<object,object>
	// System.Collections.Generic.SortedList.ValueList<double,Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.SortedList.ValueList<double,Network.Sync.TransformSnapshot>
	// System.Collections.Generic.SortedList.ValueList<double,object>
	// System.Collections.Generic.SortedList.ValueList<object,object>
	// System.Collections.Generic.SortedList<double,Common.Tools.SnapshotInterpolation.TimeSnapshot>
	// System.Collections.Generic.SortedList<double,Network.Sync.TransformSnapshot>
	// System.Collections.Generic.SortedList<double,object>
	// System.Collections.Generic.SortedList<object,object>
	// System.Collections.Generic.Stack.Enumerator<object>
	// System.Collections.Generic.Stack<object>
	// System.Collections.ObjectModel.ReadOnlyCollection<kcp2k.AckItem>
	// System.Collections.ObjectModel.ReadOnlyCollection<object>
	// System.Comparison<kcp2k.AckItem>
	// System.Comparison<object>
	// System.Func<System.Collections.Generic.KeyValuePair<long,object>,System.Collections.DictionaryEntry>
	// System.Func<System.Collections.Generic.KeyValuePair<long,object>,byte>
	// System.Func<System.Collections.Generic.KeyValuePair<long,object>,long>
	// System.Func<System.Collections.Generic.KeyValuePair<long,object>,object>
	// System.Func<long,byte>
	// System.Func<long,int>
	// System.Func<object,UnityEngine.Color32>
	// System.Func<object,byte>
	// System.Func<object,int>
	// System.Func<object,object>
	// System.Func<object>
	// System.IEquatable<object>
	// System.Linq.Buffer<object>
	// System.Linq.Enumerable.<SelectManyIterator>d__17<object,object>
	// System.Linq.Enumerable.Iterator<object>
	// System.Linq.Enumerable.WhereArrayIterator<object>
	// System.Linq.Enumerable.WhereEnumerableIterator<object>
	// System.Linq.Enumerable.WhereListIterator<object>
	// System.Nullable<System.DateTime>
	// System.Nullable<System.Decimal>
	// System.Nullable<System.Guid>
	// System.Nullable<UnityEngine.Color32>
	// System.Nullable<UnityEngine.Color>
	// System.Nullable<UnityEngine.LayerMask>
	// System.Nullable<UnityEngine.Matrix4x4>
	// System.Nullable<UnityEngine.Plane>
	// System.Nullable<UnityEngine.Quaternion>
	// System.Nullable<UnityEngine.Ray>
	// System.Nullable<UnityEngine.Rect>
	// System.Nullable<UnityEngine.Vector2>
	// System.Nullable<UnityEngine.Vector2Int>
	// System.Nullable<UnityEngine.Vector3>
	// System.Nullable<UnityEngine.Vector3Int>
	// System.Nullable<UnityEngine.Vector4>
	// System.Nullable<byte>
	// System.Nullable<double>
	// System.Nullable<float>
	// System.Nullable<int>
	// System.Nullable<long>
	// System.Nullable<object>
	// System.Nullable<sbyte>
	// System.Nullable<short>
	// System.Nullable<uint>
	// System.Nullable<ulong>
	// System.Nullable<ushort>
	// System.Predicate<UnityEngine.LowLevel.PlayerLoopSystem>
	// System.Predicate<kcp2k.AckItem>
	// System.Predicate<object>
	// System.ReadOnlySpan.Enumerator<byte>
	// System.ReadOnlySpan<byte>
	// System.Runtime.CompilerServices.ConditionalWeakTable.CreateValueCallback<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable.Enumerator<object,object>
	// System.Runtime.CompilerServices.ConditionalWeakTable<object,object>
	// System.Span.Enumerator<byte>
	// System.Span<byte>
	// }}

	public void RefMethods()
	{
		// Google.Protobuf.FieldCodec<object> Google.Protobuf.FieldCodec.ForMessage<object>(uint,Google.Protobuf.MessageParser<object>)
		// object Google.Protobuf.ProtoPreconditions.CheckNotNull<object>(object,string)
		// object System.Activator.CreateInstance<object>()
		// object[] System.Array.Empty<object>()
		// int System.Array.FindIndex<UnityEngine.LowLevel.PlayerLoopSystem>(UnityEngine.LowLevel.PlayerLoopSystem[],System.Predicate<UnityEngine.LowLevel.PlayerLoopSystem>)
		// int System.Array.FindIndex<UnityEngine.LowLevel.PlayerLoopSystem>(UnityEngine.LowLevel.PlayerLoopSystem[],int,int,System.Predicate<UnityEngine.LowLevel.PlayerLoopSystem>)
		// System.Void System.Array.Resize<UnityEngine.LowLevel.PlayerLoopSystem>(UnityEngine.LowLevel.PlayerLoopSystem[]&,int)
		// System.Void System.Array.Resize<byte>(byte[]&,int)
		// int System.HashCode.Combine<long,long,long>(long,long,long)
		// bool System.Linq.Enumerable.All<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.SelectMany<object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,System.Collections.Generic.IEnumerable<object>>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.SelectManyIterator<object,object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,System.Collections.Generic.IEnumerable<object>>)
		// object[] System.Linq.Enumerable.ToArray<object>(System.Collections.Generic.IEnumerable<object>)
		// System.Collections.Generic.IEnumerable<object> System.Linq.Enumerable.Where<object>(System.Collections.Generic.IEnumerable<object>,System.Func<object,bool>)
		// object System.Reflection.CustomAttributeExtensions.GetCustomAttribute<object>(System.Reflection.MemberInfo)
		// object UnityEngine.Component.GetComponent<object>()
		// object UnityEngine.Component.GetComponentInChildren<object>()
		// bool UnityEngine.Component.TryGetComponent<object>(object&)
		// object UnityEngine.GameObject.GetComponent<object>()
		// object UnityEngine.GameObject.GetComponentInChildren<object>(bool)
		// bool UnityEngine.GameObject.TryGetComponent<object>(object&)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Transform,bool)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion)
		// object UnityEngine.Object.Instantiate<object>(object,UnityEngine.Vector3,UnityEngine.Quaternion,UnityEngine.Transform)
	}
}