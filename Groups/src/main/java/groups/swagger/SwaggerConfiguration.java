package groups.swagger;

import org.springdoc.core.GroupedOpenApi;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

//http://localhost:8080/swagger-ui/index.html
@Configuration
public class SwaggerConfiguration {

    @Bean
    public GroupedOpenApi publicApi() {

        return GroupedOpenApi.builder()
                .group("swagger.json")
                .packagesToScan(
                        "groups.group.controller",
                        "groups.groupTrainer.controller",
                        "groups.workout.controller",
                        "groups.workoutParticipant.controller",
                        "groups.workoutPermission.controller"
                )
                .build();
    }
}
