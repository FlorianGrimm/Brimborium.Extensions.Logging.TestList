namespace Brimborium.Extensions.Logging.TestList;

public class TestLoggerSnapshotTests {
    [Test]
    public async Task CloneTest() {
        var (appServiceProvider, snapshot) = TestUtility.CreateANNA();

        _ = await Assert.That(snapshot).FindNext(new(Message: "A"));

        var snapshotCopy = snapshot.Clone();
        _ = await Assert.That(snapshot.Index).IsEqualTo(0);
        _ = await Assert.That(snapshotCopy.Index).IsEqualTo(0);
        _ = await Assert.That(snapshotCopy.GetCurrentItem()).IsSameReferenceAs(snapshot.GetCurrentItem());

        _ = await Assert.That(snapshot).FindNext(new(Message: "A"));
        _ = await Assert.That(snapshotCopy).FindNext(new(Message: "A"));
        _ = await Assert.That(snapshot.Index).IsEqualTo(3);
        _ = await Assert.That(snapshotCopy.Index).IsEqualTo(3);
        _ = await Assert.That(snapshotCopy.GetCurrentItem()).IsSameReferenceAs(snapshot.GetCurrentItem());

        appServiceProvider.Dispose();
    }


    [Test]
    public async Task GetItemByIndex_SuccessTest() {
        var (appServiceProvider, snapshot) = TestUtility.CreateANNA();

        _ = await Assert.That(snapshot).FindNext(new(Message: "A"));

        _ = await Assert.That(snapshot.GetItemByIndex(-1)).IsNull();
        _ = await Assert.That(snapshot.GetItemByIndex(0)?.Message).EqualTo("A");
        _ = await Assert.That(snapshot.GetItemByIndex(3)?.Message).EqualTo("A");
        _ = await Assert.That(snapshot.GetItemByIndex(4)).IsNull();

        appServiceProvider.Dispose();
    }
}
