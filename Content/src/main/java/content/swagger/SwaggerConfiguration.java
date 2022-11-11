package content.swagger;

import io.swagger.v3.oas.models.Components;
import io.swagger.v3.oas.models.OpenAPI;
import io.swagger.v3.oas.models.info.Info;
import io.swagger.v3.oas.models.security.SecurityRequirement;
import io.swagger.v3.oas.models.security.SecurityScheme;
import org.springdoc.core.GroupedOpenApi;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import static org.springframework.http.HttpHeaders.AUTHORIZATION;

//http://localhost:8080/swagger-ui/index.html
@Configuration
public class SwaggerConfiguration {

    @Bean
    public GroupedOpenApi publicApi() {

        return GroupedOpenApi.builder()
                .group("swagger.json")
                .packagesToScan(
                        "content.workout.controller",
                        "content.bodyPart.controller",
                        "content.criterion.controller",
                        "content.exercise.controller",
                        "content.performedWorkout.controller",
                        "content.workoutExercise.controller",
                        "content.workoutExerciseCriterionResult.controller"
                )
                .build();
    }

    @Bean
    public OpenAPI customOpenAPI() {

        return new OpenAPI()
                .info(new Info().title("Enter token"))
                .components(new Components()
                        .addSecuritySchemes(
                                "jwtToken", new SecurityScheme()
                                        .type(SecurityScheme.Type.APIKEY)
                                        .in(SecurityScheme.In.HEADER)
                                        .name(AUTHORIZATION)
                        )
                )
                .addSecurityItem(new SecurityRequirement().addList("jwtToken"));
    }
}

