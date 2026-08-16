namespace Brimborium.Extensions.Logging.TestList;

public class FindNextAssertionTests {
    [Test]
    public async Task FindNext_Success_Test() {
        var (appServiceProvider, snapshot) = TestUtility.CreateANNA();

        // Assert

        _ = await Assert.That(snapshot.GetCurrentItem()).IsNull();
        _ = await Assert.That(snapshot.GetPreviousItem()).IsNull();
        _ = await Assert.That(snapshot).FindNext(new(Message: "A"));
        _ = await Assert.That(snapshot).FindNext(new(Message: "N"));
        _ = await Assert.That(snapshot).FindNext(new(Message: "N"));
        _ = await Assert.That(snapshot).FindNext(new(Message: "A"));
        _ = await Assert.That(snapshot.GetCurrentItem()).IsNotNull();
        _ = await Assert.That(snapshot.GetPreviousItem()).IsNotNull();
        _ = await Assert.That(snapshot.GetCurrentItem()).IsNotSameReferenceAs(snapshot.GetPreviousItem());

        appServiceProvider.Dispose();
    }

    [Test]
    public async Task FindNext_1NotFound_Failed_Test() {
        var (appServiceProvider, snapshot) = TestUtility.CreateANNA();

        _ = await Assert.ThrowsAsync<Exception>(async () => {
            _ = await Assert.That(snapshot).FindNext(new(Message: "B"));
        });

        appServiceProvider.Dispose();
    }

    [Test]
    public async Task FindNext_1Found_Then_1NotFound_Failed_Test() {
        var (appServiceProvider, snapshot) = TestUtility.CreateANNA();
        _ = await Assert.That(snapshot).FindNext(new(Message: "A"));
        _ = await Assert.ThrowsAsync<Exception>(async () => {
            _ = await Assert.That(snapshot).FindNext(new(Message: "B"));
        });

        appServiceProvider.Dispose();
    }

    [Test]
    public async Task FindNext_4Found_Then_1NotFound_Failed_Test() {
        var (appServiceProvider, snapshot) = TestUtility.CreateANNA();
        // Assert

        _ = await Assert.That(snapshot).FindNext(new(Message: "A"));
        _ = await Assert.That(snapshot).FindNext(new(Message: "N"));
        _ = await Assert.That(snapshot).FindNext(new(Message: "N"));
        _ = await Assert.That(snapshot).FindNext(new(Message: "A"));
        _ = await Assert.ThrowsAsync<Exception>(async () => {
            _ = await Assert.That(snapshot).FindNext(new(Message: "B"));
        });

        appServiceProvider.Dispose();
    }
}