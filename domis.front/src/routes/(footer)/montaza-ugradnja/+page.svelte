<script lang="ts">
    interface VideoCard {
        id: string; // YouTube video ID
        url: string; // Link to the video
    }

    // List of video cards with URLs
    const videoCards: VideoCard[] = [
        { id: "TVt__of-Fn4", url: "https://www.youtube.com/watch?v=TVt__of-Fn4" },
        { id: "XdUQ-gz8IOE", url: "https://www.youtube.com/watch?v=XdUQ-gz8IOE" },
        { id: "kyC25aRVE5Y", url: "https://www.youtube.com/watch?v=kyC25aRVE5Y" },
        { id: "X-tbDetl2dY", url: "https://www.youtube.com/watch?v=X-tbDetl2dY" },
        { id: "nubO3l0CxIw", url: "https://www.youtube.com/watch?v=nubO3l0CxIw" },
        { id: "9GlXfZmLArc", url: "https://www.youtube.com/watch?v=9GlXfZmLArc" },
        { id: "OOPja-KJhC0", url: "https://www.youtube.com/watch?v=OOPja-KJhC0" },
        { id: "Obz5aD6dbxA", url: "https://www.youtube.com/watch?v=Obz5aD6dbxA" },
        { id: "7ODGDWl9lh4", url: "https://www.youtube.com/watch?v=7ODGDWl9lh4" },
        { id: "hGh7V_dCyzU", url: "https://www.youtube.com/watch?v=hGh7V_dCyzU" },
        { id: "ybgLbZamFbA", url: "https://www.youtube.com/watch?v=ybgLbZamFbA" },
        { id: "F0sYMX5n5GE", url: "https://www.youtube.com/watch?v=F0sYMX5n5GE" },
    ];
</script>

<section class="mt-4 w-full flex justify-center">
    <div class="w-11/12 xl:w-3/4 category-cards grid grid-cols-1 lg:grid-cols-3 gap-x-4 gap-y-4">
        {#each videoCards as { id, url }}
            <div class="border rounded-lg shadow-lg min-w-[300px]"> <!-- Set a minimum width -->
                <a href={url} target="_blank" rel="noopener noreferrer">
                    <iframe
                        width="100%"
                        height="315"
                        src="https://www.youtube.com/embed/{id}"
                        title="YouTube video player"
                        frameborder="0"
                        allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
                        allowfullscreen
                        class="rounded-t-lg"
                    ></iframe>
                </a>
                <div class="p-4 flex justify-center">
                    <a href={url} target="_blank" rel="noopener noreferrer" class="text-gray-500 hover:underline">
                        Gledaj
                    </a>
                </div>
            </div>
        {/each}
    </div>
</section>
